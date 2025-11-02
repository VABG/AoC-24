using AdventOfCodeCore.Models.Visualization;

namespace AdventOfCodeCore.Interfaces;

public interface ITextRenderer
{
    void DrawCharacter(CharacterAndPosition character);
    void DrawCharacters(CharacterAndPosition[] characters);
    void DrawCharacters(Character[,] characters);
    void DrawCharacters(char[,] characters);
    void DrawCharacters(char[,] characters, Color color);
    void DrawCharacter(char c, int x, int y, Color color);
    void DrawCharacter(char c, int x, int y);
    void Clear();
}