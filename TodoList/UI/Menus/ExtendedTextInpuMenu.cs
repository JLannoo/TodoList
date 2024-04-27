using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Menus;

namespace TodoList.UI.Menus;

public class ExtendedTextInputMenu<T> : TextInputMenu {
    // Override definition for doneNaming and make dummy callback for base class
    public new T doneNaming;
    private static doneNamingBehavior dummyCallback = (string str) => { };

    public delegate void DoneEvent(TextBox sender);
    public event DoneEvent OnDoneButtonClicked;

    public ExtendedTextInputMenu(T callback, string title, string defaultValue = "") : base(dummyCallback, title, defaultValue) {
        doneNamingButton.setPosition(doneNamingButton.bounds.Location.ToVector2() + new Vector2(0, 100));

        doneNaming = callback;
        textBox.OnEnterPressed += textBoxEnter;
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true) {
        if(upperRightCloseButton.containsPoint(x, y)) {
            Game1.activeClickableMenu = ModEntry.menu;
            Game1.playSound("bidDeSelect");
            return;
        }

        if(doneNamingButton.containsPoint(x, y)) {
            OnDoneButtonClicked?.Invoke(textBox);
            return;
        }

        base.receiveLeftClick(x, y, playSound);
    }
}