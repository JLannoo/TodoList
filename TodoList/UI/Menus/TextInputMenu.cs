using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace TodoList.UI.Menus;

public class TextInputMenu : NamingMenu {
    public TextInputMenu(doneNamingBehavior callback, string title, string defaultName = "") : base(callback, title, defaultName) {
        textBox.Width = Game1.uiViewport.Width/2;
        textBox.Selected = false;
        textBox.limitWidth = false;
        textBox.textLimit = 90;
        textBox.X = Game1.uiViewport.Width/2 - textBox.Width / 2;

        // Move doneNamingButton
        doneNamingButton.setPosition(new Vector2(
            width / 2 - doneNamingButton.bounds.Width / 2,
            height / 2 + textBox.Height / 2
        ));

        // Hide randomButton
        randomButton = new ClickableTextureComponent(Rectangle.Empty, Game1.menuTexture, Rectangle.Empty, 0);

        initializeUpperRightCloseButton();
        upperRightCloseButton.setPosition(new Vector2(
           Game1.uiViewport.Width * 0.75f,
           Game1.uiViewport.Height / 2 - 128
        ));
    }

    public void open() {
        textBox.Selected = true;
        Game1.activeClickableMenu = this;
        Game1.playSound("bigSelect");
    }

    public override void draw(SpriteBatch b) {
        base.draw(b);
        upperRightCloseButton.draw(b);
        drawMouse(b);
    }

    public override void releaseLeftClick(int x, int y) {
        base.releaseLeftClick(x, y);

        if(upperRightCloseButton.containsPoint(x, y)) {
            Game1.activeClickableMenu = ModEntry.menu;
        }
    }
}