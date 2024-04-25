using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;

namespace TodoList.UI.Components;

public class TodoTab : ClickableTextureComponent {
    public static readonly Point tabSize = new(16, 16);

    public readonly List<TodoItem> items = new();

    public bool active = false;

    public TodoTab(string name, Rectangle bounds, string label, string hoverText, Texture2D texture, Rectangle sourceRect, float scale, bool drawShadow = false) : base(name, bounds, label, hoverText, texture, sourceRect, scale, drawShadow) {

    }

    public override void draw(SpriteBatch b) {
        base.draw(b, Color.White, 1, xOffset: active ? 10 : 0);
    }
}