using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace TodoList.UI.Components;

public class CreateTabButton : ClickableTextureComponent {
    private static ClickableTextureComponent emptyTab;
    private static ClickableTextureComponent plusSign;

    public CreateTabButton(Vector2 position) 
        : base(
            "CreateTab", 
            new Rectangle(position.ToPoint(), new Point(64)), 
            "", 
            "Create new tab", 
            Game1.mouseCursors, 
            Rectangle.Empty, 
            0
        )
    {
        emptyTab = new(
            new Rectangle(
                position.ToPoint(),
                new Point(64)
            ),
            Game1.mouseCursors,
            new Rectangle(16, 368, 16, 16),
            4f
        );

        plusSign = new(
            new Rectangle(
                position.ToPoint(),
                new Point(64)
            ),
            Game1.mouseCursors,
            new Rectangle(0, 410, 16, 16),
            2.5f
        );
    }

    public override void draw(SpriteBatch b) {
        b.Draw(
            emptyTab.texture,
            new Rectangle(emptyTab.bounds.Center, new Point(emptyTab.bounds.Width)),
            emptyTab.sourceRect,
            Color.White,
            (float)-Math.PI / 2,
            new Vector2(emptyTab.bounds.Width / 2 / emptyTab.scale),
            SpriteEffects.None,
            1
        );

        plusSign.draw(b, Color.White, 1, 0, 17, 10);
    }

    new public void setPosition(Vector2 position) {
        bounds.X = (int)position.X;
        bounds.Y = (int)position.Y;

        emptyTab.setPosition(position);
        plusSign.setPosition(position);
    }
}