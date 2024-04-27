using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using static StardewValley.Menus.CoopMenu;
using TodoList.Extensions;
using TodoList.UI.Menus;

namespace TodoList.UI.Components;

public class TodoTab : ClickableTextureComponent {
    public static readonly Point tabSize = new(16, 16);
    public static readonly float Scale = 4f;

    public readonly List<TodoItem> items = new();
    public readonly TodoMenu parentMenu;
    public readonly int index;

    public bool active = false;

    public TodoTab(TodoMenu parentMenu, Vector2 position, string name, TabIconNames icon)
    : base(
        name,
        new Rectangle(position.ToPoint(), tabSize.Multiply(Scale)),
        "",
        name,
        Game1.mouseCursors,
        TodoMenu.TabIconsSourceRects[icon],
        Scale,
        drawShadow: true
    ) {
        this.parentMenu = parentMenu;
        index = parentMenu.tabs.Count;
    }

    public override void draw(SpriteBatch b) {
        base.draw(b, Color.White, 1, xOffset: active ? 10 : 0);
    }

    public void deleteItem(int index) {
        items.Remove(items.Find(i => i.index == index));
        Game1.playSound("trashcan");
    }
}