using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;

namespace TodoList.UI;

public class TodoItem : ClickableTextureComponent {
    public TodoItem(string name, Rectangle bounds, string label, string hoverText, Texture2D texture, Rectangle sourceRect, float scale, bool drawShadow = false) : base(name, bounds, label, hoverText, texture, sourceRect, scale, drawShadow) {
    }
}
