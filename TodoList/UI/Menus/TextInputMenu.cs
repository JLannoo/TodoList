using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Menus;

namespace TodoList.UI.Menus;

public class TextInputMenu : NamingMenu {
    public TextInputMenu(doneNamingBehavior b, string title, string defaultName = null) : base(b, title, defaultName) {
        textBox.Width = Game1.uiViewport.Width/2;
        textBox.Selected = false;
        textBox.limitWidth = false;
        textBox.textLimit = 90;
        textBox.X = Game1.uiViewport.Width/2 - textBox.Width / 2;

        doneNamingButton.setPosition(new Vector2(
            width / 2 - doneNamingButton.bounds.Width / 2,
            height / 2 + textBox.Height / 2
        ));

        randomButton = new ClickableTextureComponent(Rectangle.Empty, Game1.menuTexture, Rectangle.Empty, 0);
    }
}