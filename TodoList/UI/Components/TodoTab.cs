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
    public readonly int index;
    public readonly TabIconNames iconName;

    public bool active = false;

    public TodoTab(Vector2 position, string name, TabIconNames icon)
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
        index = ModEntry.modData.tabs.Count;
        iconName = icon;
    }

    public TodoTab(SerializedTab tab)
    : base(
        tab.name,
        new Rectangle(tab.position.ToPoint(), tabSize.Multiply(Scale)),
        "",
        tab.name,
        Game1.mouseCursors,
        TodoMenu.TabIconsSourceRects[tab.iconName],
        Scale,
        drawShadow: true
    ) {
        index = tab.index;
        active = tab.active;
        iconName = tab.iconName;
    }

    public override void draw(SpriteBatch b) {
        base.draw(b, Color.White, 1, xOffset: active ? 10 : 0);
    }

    public void deleteItem(int index) {
        items.Remove(items.Find(i => i.index == index));
        Game1.playSound("trashcan");
    }
}