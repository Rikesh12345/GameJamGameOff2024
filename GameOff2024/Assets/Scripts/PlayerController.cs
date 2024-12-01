using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("UnitySetup")]
    private PlayerControls _playerControls;
    private Vector2 _moveInput;
    private Vector2 _smoothedMovementInput;
    private Vector2 _movementInputSmoothVelocity;
    private float _currentRotationVelocity; // Helps to smooth rotation

    private Rigidbody2D _rigidbody2D;

    private float _currrentSpeed;
    private Vector3 _lastPos;

    private Unit _unit;

    private bool _canMove = false;
    public bool CanMove { get => _canMove; set => _canMove = value; }
    private bool _isCutsceneActive = false;
    public bool IsCutsceneActive { get => _isCutsceneActive; set => _isCutsceneActive = value; }

    [Header("Salvage Mechanic")]
    private GameObject _salvageUIGO;
    private SalvageUI _salvageUI;
    private bool _canSalvage = false;
    private bool _isSalvaging = false;

    private bool _isSailingMode = false; // Indicates if the ship is sailing automatically.
    public bool IsSailingMode { get => _isSailingMode; private set => _isSailingMode = value; }

    private void Awake()
    {
        try
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();

            _playerControls = new PlayerControls();

            _unit = GetComponent<Unit>();

            _playerControls.Player.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
            _playerControls.Player.Move.canceled += ctx => _moveInput = Vector2.zero;

            _playerControls.Player.Salvage.started += _ => StartSalvaging();
            _playerControls.Player.Salvage.canceled += _ => StopSalvaging();
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Awake: " + ex);
        }

    }

    private void OnEnable()
    {
        try
        {
            _playerControls.Player.Enable();
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".OnEnable: " + ex);
        }
    }

    private void Start()
    {
        try
        {
            _salvageUIGO = GameManager.Instance.SalvageUIGO;
            _salvageUI = GameManager.Instance.SalvageUI;
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".OnEnable: " + ex);
        }
    }

    private void Update()
    {
        //ShowVelocity();

        try
        {
            if (_unit.Dead)
            {
                return;
            }

            // Toggle sailing mode with "Shift"
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                IsSailingMode = !IsSailingMode; // Toggle sailing mode
            }

            // Interrupt sailing mode with "W" or "S"
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
            {
                IsSailingMode = false;
            }

            // In sailing mode, only force forward movement while allowing horizontal input for turning
            if (IsSailingMode)
            {
                _moveInput = new Vector2(_moveInput.x, 1f); // Keep horizontal input and override forward movement
            }

            if (_isSalvaging && _canSalvage)
            {
                _salvageUI.IsSalvaging = true;
            }
            else
            {
                _salvageUI.IsSalvaging = false;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".Update: " + ex);
        }
    }

    private void StartSalvaging()
    {
        try
        {
            if (_canSalvage)
            {
                // Increment salvage progress only on a single key press
                SalvageZone salvageZone = _salvageUI.SalvageZoneGO.GetComponent<SalvageZone>();
                if (salvageZone != null)
                {
                    float fillRate = salvageZone.FillRate;
                    _salvageUI.UpdateFill(fillRate); // Add zone-specific fill rate
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".StartSalvaging: " + ex);
        }
    }

    private void StopSalvaging()
    {
        try
        {
            _isSalvaging = false;
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".StopSalvaging: " + ex);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        try
        {
            if (other.CompareTag("SalvageZone"))
            {
                _salvageUIGO.SetActive(true);
                _canSalvage = true;
                _salvageUI.SalvageZoneGO = other.gameObject;
                SalvageZone salvageZone = other.gameObject.GetComponent<SalvageZone>();
                _salvageUI.SalvageZone = salvageZone;
                GameManager.Instance.SetupLootWindow(salvageZone.GoldCoins, salvageZone.ConstructionSprite, salvageZone.ConnstructionDescriptionText, salvageZone.UpgradeSprite, salvageZone.UpgradeDescriptionText);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".OnTriggerEnter2D: " + ex);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        try
        {
            if (other.CompareTag("SalvageZone"))
            {
                _canSalvage = false;
                _salvageUI.ResetUI();
                _salvageUIGO.SetActive(false);
                _salvageUI.SalvageZoneGO = null;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".OnTriggerExit2D: " + ex);
        }
    }

    private void ShowVelocity()
    {
        try
        {
            Vector3 currentMovementVector = transform.position - _lastPos;
            _currrentSpeed = currentMovementVector.sqrMagnitude / Time.deltaTime;
            _lastPos = transform.position;

            if (_currrentSpeed != 0)
            {
                Debug.Log("Velocity: " + _currrentSpeed);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".ShowVelocity: " + ex);
        }
    }

    private void FixedUpdate()
    {
        try
        {
            if (_unit.Dead || IsCutsceneActive)
            {
                _rigidbody2D.velocity = Vector2.zero;
                _rigidbody2D.angularVelocity = 0f;
                return;
            }

            if (!_unit.IsKnockedBack && CanMove)
            {
                //100 Rotationspeed 
                PerspectiveRigidbodyMovement();
                PerspectiveRotateInDirectionOfInput();

                //150 Rotationspeed 
                //RigidbodyMovement();
                //RotateInDirectionOfInput();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".FixedUpdate: " + ex);
        }
    }

    private void PerspectiveRigidbodyMovement()
    {
        try
        {
            // Calculate local movement based on input
            Vector2 forwardMovement = transform.up * _moveInput.y;   // "W" and "S" for forward/backward
            Vector2 lateralMovement = transform.right * _moveInput.x; // "A" and "D" for left/right

            // Combine forward and lateral movement
            Vector2 movement = forwardMovement + lateralMovement;

            // Smooth the movement for a less abrupt change in velocity
            _smoothedMovementInput = Vector2.SmoothDamp(_smoothedMovementInput, movement, ref _movementInputSmoothVelocity, 1.5f);

            // Apply the smoothed movement to the Rigidbody2D velocity
            _rigidbody2D.velocity = _smoothedMovementInput * _unit.MovementSpeed;
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".RigidbodyMovement: " + ex);
        }
    }

    private void PerspectiveRotateInDirectionOfInput()
    {
        try
        {
            // Check if there's left/right input
            if (_moveInput.x != 0)
            {
                // Determine rotation direction: right (D) or left (A)
                float targetAngle = _moveInput.x > 0 ? -_unit.RotationSpeed : _unit.RotationSpeed;

                // Instantly reset rotation velocity when changing direction to avoid "overshoot" or delay
                _currentRotationVelocity = 0;

                // Smooth rotation towards the target angle, directly adjusting the Rigidbody2D's rotation
                float smoothedRotation = Mathf.LerpAngle(
                    _rigidbody2D.rotation,
                    _rigidbody2D.rotation + targetAngle,
                    0.01f // Lerp factor for a smooth but quick response, adjust if needed
                );

                _rigidbody2D.MoveRotation(smoothedRotation);
            }
            else
            {
                // Slow down rotation when no direction is being pressed, for a smoother stopping effect
                float smoothedRotation = Mathf.SmoothDampAngle(
                    _rigidbody2D.rotation,
                    _rigidbody2D.rotation,
                    ref _currentRotationVelocity,
                    0.06f // Stopping smoothness
                );

                _rigidbody2D.MoveRotation(smoothedRotation);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".RotateInDirectionOfInput: " + ex);
        }
    }

    //// Knockback coroutine to disable movement temporarily
    //public IEnumerator ApplyKnockback(Vector2 force)
    //{
    //    try
    //    {
    //        _isKnockedBack = true;

    //        // Apply the knockback force
    //        _rigidbody2D.velocity = Vector2.zero; // Stop current movement
    //        _rigidbody2D.AddForce(force, ForceMode2D.Impulse);

    //        Debug.Log($"Knockback applied: {force}");
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.LogError("ERROR in ApplyKnockback: " + ex);
    //    }

    //    // Wait for the knockback duration
    //    yield return new WaitForSeconds(_knockbackDuration);

    //    _isKnockedBack = false;
    //    Debug.Log("Knockback ended");
    //}

    //public void DisableMovement()
    //{
    //    try
    //    {
    //        _rigidbody2D.isKinematic = true;
    //        CanMove = false;
    //        _rigidbody2D.velocity = Vector2.zero;
    //        _rigidbody2D.angularVelocity = 0f;
    //        IsCutsceneActive = true;
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.LogError("ERROR in " + this + ".DisableMovement: " + ex);
    //    }
    //}

    //public void EnableMovement()
    //{
    //    try
    //    {
    //        _rigidbody2D.velocity = Vector2.zero;
    //        _rigidbody2D.angularVelocity = 0f;
    //        CanMove = true;
    //        _rigidbody2D.isKinematic = false;
    //        IsCutsceneActive = true;
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.LogError("ERROR in " + this + ".EnableMovement: " + ex);
    //    }
    //}

    public void DisableMovement()
    {
        try
        {
            _rigidbody2D.velocity = Vector2.zero;         // Stop all linear movement
            _rigidbody2D.angularVelocity = 0f;           // Stop all angular movement
            _rigidbody2D.isKinematic = true;             // Disable physics simulation
            CanMove = false;                             // Prevent player inputs
            IsCutsceneActive = true;                     // Flag for cutscene
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".DisableMovement: " + ex);
        }
    }

    public void EnableMovement()
    {
        try
        {
            // Reset Rigidbody to ensure no residual movement
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.angularVelocity = 0f;

            // Reactivate physics and player controls
            _rigidbody2D.isKinematic = false;            // Re-enable physics simulation
            //CanMove = true;                              // Allow player inputs
            IsCutsceneActive = false;                    // Flag to indicate cutscene end
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".EnableMovement: " + ex);
        }
    }




    //private void RigidbodyMovement()
    //{
    //    try
    //    {
    //        Vector2 movement = new Vector2(_moveInput.x, _moveInput.y);

    //        _smoothedMovementInput = Vector2.SmoothDamp(_smoothedMovementInput, movement, ref _movementInputSmoothVelocity, 1.5f);

    //        _rigidbody2D.velocity = _smoothedMovementInput * _ship.MovementSpeed;
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.LogError("ERROR in " + this + ".RigidbodyMovement: " + ex);
    //    }
    //}

    //private void RotateInDirectionOfInput()
    //{
    //    try
    //    {
    //          150 Rotationspeed 
    //        if (_moveInput != Vector2.zero)
    //        {
    //            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _smoothedMovementInput);
    //            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _ship.RotationSpeed * Time.deltaTime);

    //            _rigidbody2D.MoveRotation(rotation);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.LogError("ERROR in " + this + ".RotateInDirectionOfInput: " + ex);
    //    }
    //}

    private void OnDisable()
    {
        try
        {
            _playerControls.Player.Disable();
        }
        catch (Exception ex)
        {
            Debug.LogError("ERROR in " + this + ".OnDisable: " + ex);
        }
    }
}
