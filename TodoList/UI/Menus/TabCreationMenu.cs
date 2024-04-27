
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace TodoList.UI.Menus;

public delegate void doneCreatingTabBehavior(string name, TabIconNames iconName);

public class TabCreationMenu : ExtendedTextInputMenu<doneCreatingTabBehavior> {
    // Override definition for doneNaming and make dummy callback for base class
    public new doneCreatingTabBehavior doneNaming;

    private List<ClickableTextureComponent> Icons = new();
    public ClickableTextureComponent selectedIcon;
    public int selectedIconIndex;


    private ClickableTextureComponent selectionBox;

    public TabCreationMenu(doneCreatingTabBehavior callback) : base(callback, "New Tab") {
        doneNamingButton.setPosition(doneNamingButton.bounds.Location.ToVector2() + new Vector2(0, 100));

        doneNaming = callback;

        foreach(var pair in TodoMenu.TabIconsSourceRects) {
            int totalIconsWidth = (pair.Value.Width * 4 + 20) * TodoMenu.TabIconsSourceRects.Count;

            Icons.Add(new(
                pair.Key.ToString(),
                new Rectangle(
                    Game1.uiViewport.Width/2 - totalIconsWidth/2 + (pair.Value.Width * 4 + 20) * Icons.Count, 
                    Game1.uiViewport.Height/2 + 100, 
                    64, 64
                ),
                "",
                pair.Key.ToString(),
                Game1.mouseCursors,
                pair.Value,
                4
            ));
        };

        selectedIcon = Icons[selectedIconIndex];

        selectionBox = new ClickableTextureComponent(
            GetSelectionBoxBounds(selectedIcon),
            Game1.mouseCursors,
            new Rectangle(64, 192, 64, 64),
            1.3f
        );

        textBox.OnEnterPressed += onDoneCreating;
        OnDoneButtonClicked += onDoneCreating;
    }

    public override void draw(SpriteBatch b) {
        base.draw(b);
        
        for(var i = 0; i < Icons.Count; i++) {
            var icon = Icons[i];
            icon.draw(b);
        }

        selectionBox?.draw(b);

        drawMouse(b);
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true) {
        base.receiveLeftClick(x, y, playSound);

        for (var i = 0; i < Icons.Count; i++) {
            var icon = Icons[i];
            if(icon.containsPoint(x, y)) {
                selectedIconIndex = i;
                selectedIcon = icon;

                selectionBox.setPosition(GetSelectionBoxBounds(icon).Location.ToVector2());
                Game1.playSound("smallSelect", 3000);
            }
        }
    }

    public void onDoneCreating(TextBox sender) {
        if (sender.Text.Length >= minLength) {
            doneNaming(sender.Text, Utils.GetEnumFromString<TabIconNames>(selectedIcon.name));
            Game1.playSound("bigSelect");
        }
    }

    private Rectangle GetSelectionBoxBounds(ClickableTextureComponent icon) {
        return new Rectangle(icon.bounds.X - 8, icon.bounds.Y - 9, (int)(64 * 1.3f), (int)(64 * 1.3f));
    }
}