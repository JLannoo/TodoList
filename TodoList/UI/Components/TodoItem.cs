using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace TodoList.UI.Components;

public class TodoItem : ClickableTextureComponent {
    public bool completed = false;
    public string text;
    public Vector2 textSize;

    public Vector2 position = Vector2.Zero;

    public TodoItem(string text) 
        : base(
            new Rectangle(0,0, (int)Game1.smallFont.MeasureString(text).X, (int)Game1.smallFont.MeasureString(text).Y),
            Game1.menuTexture,
            Rectangle.Empty,
            1
        ) {

        this.text = text;
        textSize = Game1.smallFont.MeasureString(text);
    }

    public override void draw(SpriteBatch b) {
        string checkbox = "[" + (completed ? "X" : " ") + "] ";
        Utility.drawTextWithShadow(b, checkbox + text, Game1.smallFont, position, Game1.textColor);
    }

    public void receiveLeftClick(int x, int y) {
        if(containsPoint(x, y)) {
            completed = !completed;
        };
    }

    public void updateBounds(Point position) {
        string checkbox = "[" + (completed ? "X" : " ") + "] ";
        bounds = new Rectangle(position, Game1.smallFont.MeasureString(checkbox + text).ToPoint());
    }
}
