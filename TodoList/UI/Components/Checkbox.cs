using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace TodoList.UI.Components; 

public class Checkbox : ClickableTextureComponent {
    private static Rectangle sourceRectUnchecked = new(227, 425, 9, 9);
    private static Rectangle sourceRectChecked = new(236, 425, 9, 9);
    private static int size = 36;

    public Vector2 position;

    public bool Checked { get; set; } = false;

    public Checkbox(Vector2 position, bool @checked) :
        base(
            new Rectangle(position.ToPoint(), new Point(size)),
            Game1.mouseCursors,
            @checked ? sourceRectChecked : sourceRectUnchecked,
            3f,
            drawShadow: true
        )
    {
        Checked = @checked;
        this.position = position;
    }

    public void updatePosition(Vector2 pos) {
        position = pos;
        bounds = new Rectangle(position.ToPoint(), new Point(size));
    }

    public override void draw(SpriteBatch b) {
        b.Draw(
            Game1.mouseCursors,
            new Rectangle(position.ToPoint(), new Point(size)),
            Checked ? sourceRectChecked : sourceRectUnchecked,
            Color.White
        );
    }
}
