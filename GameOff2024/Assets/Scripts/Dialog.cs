[System.Serializable]
public class Dialog
{
    private string _speaker;
    private string _text;
    public string Speaker { get => _speaker; set => _speaker = value; }
    public string Text { get => _text; set => _text = value; }

    public Dialog(string speaker, string text)
    {
        Speaker = speaker;
        Text = text;
    }


}
