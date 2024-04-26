using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace TodoList.UI.Components;

public class HoverButton : ClickableTextureComponent {
    public Vector2 position = Vector2.Zero;

    public Rectangle buttonSourceRect = new(322, 498, 12, 12);
    public int size = 12;
    public float Scale = 2.5f;

    public delegate void onClickAction();
    public onClickAction onclick;

    public HoverButton(onClickAction onClick, Rectangle iconSourceRectangle, int size, float Scale) :
        base(
            new Rectangle(new Point(0), new Point((int)(size * Scale))),
            Game1.mouseCursors,
            iconSourceRectangle,
            4f
        ) {

        onclick = onClick;
        buttonSourceRect = iconSourceRectangle;
        this.size = size;
        this.Scale = Scale;
        visible = false;
    }

    public override void draw(SpriteBatch b) {
        if (!visible) return;

        b.Draw(
            Game1.mouseCursors,
            new Rectangle(position.ToPoint(), new Point(size * (int)Scale)),
            buttonSourceRect,
            Color.White
        );
    }

    /// <summary>
    /// For debugging
    /// </summary>
    public void drawBoundsSquare(SpriteBatch b) {
        if (!visible) return;

        Utility.DrawSquare(b, new Rectangle(position.ToPoint(), new Point(size * (int)Scale)), 1, Color.Red);
    }

    public void updatePosition(Vector2 pos) {
        position = pos;
        bounds = new Rectangle(pos.ToPoint(), new Point(size * (int)Scale));
    }
}
