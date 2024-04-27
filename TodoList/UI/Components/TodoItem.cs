using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using TodoList.UI.Menus;

namespace TodoList.UI.Components;

public class TodoItem : ClickableTextureComponent {
    public TodoTab parentTab;
    public string text;
    public int index;

    public bool completed = false;

    public Vector2 textSize;

    public Checkbox checkbox;

    public List<HoverButton> actions = new();

    public Vector2 position = Vector2.Zero;
    public Vector2 textPosition = Vector2.Zero;

    private Vector2 displacementForCheckbox = new(40, 0);

    public TodoItem(TodoTab tab, string text)
        : base(
            new Rectangle(0,0, (int)Game1.smallFont.MeasureString(text).X, (int)Game1.smallFont.MeasureString(text).Y),
            Game1.menuTexture,
            Rectangle.Empty,
            1
        ) {

        parentTab = tab;
        this.text = text;
        index = parentTab.items.Count;

        textSize = Game1.smallFont.MeasureString(text);

        checkbox = new Checkbox(position, completed);

        actions.Add(new HoverButton(onDeleteButtonClick, new(322, 498, 12, 12), 12, 2.5f));
        actions.Add(new HoverButton(onRenameButtonClick, new(32, 672, 16, 16), 12, 2f));
    }

    #region Overrides
    public override void draw(SpriteBatch b) {
        drawComponents(b);

        Utility.drawTextWithShadow(b, text, Game1.smallFont, textPosition, Game1.textColor);

        if (completed) {
            Utility.drawLineWithScreenCoordinates(
                (int)textPosition.X,
                (int)(textPosition.Y + textSize.Y / 2),
                (int)(textPosition.X + textSize.X),
                (int)(textPosition.Y + textSize.Y / 2),
                b,
                Game1.textColor
            );
        }
    }

    public override void tryHover(int x, int y, float _ = 0) {
        foreach(HoverButton button in actions) {
            button.visible = containsPoint(x,y);
        }
    }
    #endregion

    #region Methods
    #region Interaction
    public void receiveLeftClick(int x, int y) {
        foreach(HoverButton button in actions) {
            if(button.visible && button.containsPoint(x, y)) {
                button.onclick();
                return;
            }
        }

        if(containsPoint(x, y)) {
            completed = !completed;
            checkbox.Checked = !checkbox.Checked;

            Game1.playSound("bigSelect", 4000);
        };

    }
    #endregion

    #region Updates
    private void drawComponents(SpriteBatch b) {
        checkbox.draw(b);

        foreach (HoverButton button in actions) {
            button.draw(b);
        }
    }

    public void updateText(string str) {
        text = str;
        textSize = Game1.smallFont.MeasureString(text);
    }

    public void updatePosition(Vector2 pos) {
        textSize = Game1.smallFont.MeasureString(text);
        position = pos;
        textPosition = pos + displacementForCheckbox;

        updateComponentsPosition(position);

        float hoverButtonsWidth = actions.Aggregate(0f, (acc, button) => acc += button.size + button.Scale);

        bounds = new Rectangle(pos.ToPoint(), (textSize + displacementForCheckbox + new Vector2(hoverButtonsWidth + actions.Count() * 20, 0)).ToPoint());
    }

    private void updateComponentsPosition(Vector2 pos) {
        checkbox.updatePosition(pos);

        float xDisplacement = pos.X + displacementForCheckbox.X + textSize.X + 20;
        for(var i = 0; i < actions.Count(); i++) {
            HoverButton button = actions[i];
            
            float previousButtonWidth = i > 0 ? (actions[i-1].size * actions[i-1].Scale) : 0;
            xDisplacement += previousButtonWidth;

            button.updatePosition(new Vector2(xDisplacement, pos.Y));
        }

    }
    #endregion

    #region Events
    private void onNameChange(string str) {
        updateText(str);
        Game1.activeClickableMenu = ModEntry.menu;
    }

    private void onDeleteButtonClick() {
        parentTab.deleteItem(index);
    }

    private void onRenameButtonClick() {
        TextInputMenu menu = new(onNameChange, "Change", text);
        menu.open();
    }
    #endregion
    #endregion
}
