﻿using StardewValley;
using StardewModdingAPI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;

namespace TodoList.UI;

public static class Utils {
    public static void DrawTextWithBox(SpriteBatch b, Vector2 position, string text, SpriteFont font, Vector2 padding, bool centerToPosition = false) {
        position -= new Vector2(0, 5);

        var textSize = font.MeasureString(text);
        var windowSize = textSize + padding * 2;

        var windowPosition = centerToPosition ? position - windowSize / 2 : position;
        var textPosition = windowPosition + padding + new Vector2(0, 5);

        IClickableMenu.drawTextureBox(b, (int)windowPosition.X, (int)windowPosition.Y, (int)windowSize.X, (int)windowSize.Y, Color.White);
        Utility.drawTextWithShadow(b, text, font, textPosition, Game1.textColor);
    }
}