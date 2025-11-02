using System.Collections.Generic;
using AdventOfCodeCore.Interfaces;
using AdventOfCodeCore.Models.Visualization;

namespace AdventOfCodeUI.ViewModels.Rendering;

public class TextRenderer : ITextRenderer
{
    public Character[,] Text { get; }
    private int _width;
    private int _height;
    
    public TextRenderer(int width, int height)
    {
        _width = width;
        _height = height;
        
        Text = new Character[_height, _width];
        for (var y = 0; y < _height; y++)
            for (int x = 0; x < _width; x++)
                Text[y,x] = new Character(' ', Colors.White);
    }

    public void DrawCharacter(CharacterAndPosition character)
    {
        if (character.X < 0 || character.X >= _width || character.Y < 0 || character.Y >= _height)
            return;
        Text[character.Y,  character.X] =  new Character(character.Char, character.Color);
    }

    public void DrawCharacters(CharacterAndPosition[] characters)
    {
        foreach (var character in characters)
            DrawCharacter(character);
    }

    public void DrawCharacters(Character[,] characters)
    {
        throw new System.NotImplementedException();
    }

    public void DrawCharacters(char[,] characters)
    {
        throw new System.NotImplementedException();
    }

    public void DrawCharacters(char[,] characters, Color color)
    {
        throw new System.NotImplementedException();
    }

    public void DrawCharacter(char c, int x, int y, Color color)
    {
        throw new System.NotImplementedException();
    }

    public void DrawCharacter(char c, int x, int y)
    {
        throw new System.NotImplementedException();
    }

    public void DrawCharacter(char c, Color color)
    {
        throw new System.NotImplementedException();
    }

    public void DrawCharacter(char c)
    {
        throw new System.NotImplementedException();
    }

    public void Clear()
    {
        for (int y = 0; y < _height; y++)
            for (int x = 0; x < _width; x++)
                Text[y,x] = new Character(' ', Colors.White);
    }
}