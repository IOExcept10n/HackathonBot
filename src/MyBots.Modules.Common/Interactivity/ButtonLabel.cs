using System.Diagnostics.CodeAnalysis;

namespace MyBots.Modules.Common.Interactivity;

public readonly struct ButtonLabel : 
    IEquatable<ButtonLabel>,
    System.Numerics.IEqualityOperators<ButtonLabel, ButtonLabel, bool>,
    ISpanParsable<ButtonLabel>
{
    private const int EmojiLength = 2;

    private readonly string _textRepresentation;

    public readonly Emoji Emoji;
    public readonly string Message;

    public ButtonLabel(Emoji emoji, string message)
    {
        Emoji = emoji;
        Message = message;
        _textRepresentation = $"{Emoji.ToUnicode()} {Message}";
    }

    private ButtonLabel(ReadOnlySpan<char> text)
    {
        _textRepresentation = text.ToString();
        if (text.Length > EmojiLength)
        {
            Emoji = text[..EmojiLength].ToEmoji(); // We assume that Telgram API supports only two-codepoints' emojis
            Message = text[(EmojiLength + 1)..].ToString();
        }
        else
        {
            Emoji = Emoji.None;
            Message = _textRepresentation;
        }
    }

    public bool Matches(string text) => text == _textRepresentation;

    public override string ToString() => _textRepresentation;

    public bool Equals(ButtonLabel other) => _textRepresentation == other._textRepresentation;

    public static implicit operator ButtonLabel(string text) => new(text);

    public override bool Equals(object? obj) => obj is ButtonLabel label && Equals(label);

    public static ButtonLabel Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => new(s);

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out ButtonLabel result)
    {
        result = new(s);
        return true;
    }

    public static ButtonLabel Parse(string s, IFormatProvider? provider) => Parse(s.AsSpan(), provider);

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out ButtonLabel result)
        => TryParse(s != null ? s.AsSpan() : [], provider, out result);

    public static bool operator ==(ButtonLabel left, ButtonLabel right) => left.Equals(right);

    public static bool operator !=(ButtonLabel left, ButtonLabel right) => !(left == right);

    public override int GetHashCode() => _textRepresentation?.GetHashCode() ?? 0;
}
