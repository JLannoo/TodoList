using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using TodoList.UI.Components;
using xTile.Dimensions;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace TodoList.UI.Menus;

public enum TabIconNames {
    Fish,
    Artifact,
    Mineral,
    Dish,
    Crop,
    Star,
    Note,
    Letter,
};

public class TodoMenu : IClickableMenu {
    public  event EventHandler ListChange;

    private static readonly Size size = new(Game1.uiViewport.Width - 300, Game1.uiViewport.Height - 200);
    private static readonly Point tabDisplacementToOrigin = new(-TodoTab.tabSize.X, TodoTab.tabSize.Y);
    private static readonly Point itemsDisplacementToOrigin = new(40, 20);

    public static readonly Dictionary<TabIconNames, Rectangle> TabIconsSourceRects = new() {
        { TabIconNames.Fish,     new Rectangle(640, 64, 16, 16) },
        { TabIconNames.Artifact, new Rectangle(656, 64, 16, 16) },
        { TabIconNames.Mineral,  new Rectangle(672, 64, 16, 16) },
        { TabIconNames.Dish,     new Rectangle(688, 64, 16, 16) },
        { TabIconNames.Crop,     new Rectangle(640, 80, 16, 16) },
        { TabIconNames.Star,     new Rectangle(656, 80, 16, 16) },
        { TabIconNames.Note,     new Rectangle(672, 80, 16, 16) },
        { TabIconNames.Letter,   new Rectangle(688, 80, 16, 16) },
    };

    private Point tabAnchorPoint;
    private Point itemsAnchorPoint;

    private string? hoverText;

    public CreateTab createTabButton;

    public TodoTab? currentTab;

    public TodoMenu(bool playOpeningSound = true)
        : base(
            (Game1.uiViewport.Width - size.Width) / 2,
            (Game1.uiViewport.Height - size.Height) / 2,
            size.Width, size.Height,
            showUpperRightCloseButton: true
        ) {
        if (playOpeningSound) {
            Game1.playSound("bigSelect");
        }

        tabAnchorPoint = new(xPositionOnScreen + spaceToClearSideBorder + tabDisplacementToOrigin.X * (int)TodoTab.Scale, yPositionOnScreen + spaceToClearTopBorder);
        itemsAnchorPoint = new(xPositionOnScreen + spaceToClearSideBorder + itemsDisplacementToOrigin.X, yPositionOnScreen + spaceToClearTopBorder + itemsDisplacementToOrigin.Y);

        createTabButton = new CreateTab(GetNewTabPosition());
    }

    #region Methods
    #region Overrides
    public override void draw(SpriteBatch b) {
        base.draw(b);
        Game1.drawDialogueBox(xPositionOnScreen, yPositionOnScreen, width, height, speaker: false, drawOnlyBox: true);

        foreach (var tab in ModEntry.modData.tabs) {
            tab.draw(b);

            if (ModEntry.modData.lastActiveTab == tab.index) {
                tab.active = true;
                currentTab = tab;
            }
        }
        createTabButton.draw(b);

        if(currentTab != null) {
            var pendingItems = currentTab.items.FindAll(item => !item.completed);
            var completedItems = currentTab.items.FindAll(item => item.completed);

            // Tracks in which height it should draw
            Vector2 currentDisplacement = Vector2.Zero;

            if(pendingItems.Count > 0) {
                DrawTitle(b, "To-do", ref currentDisplacement);
                DrawItems(b, pendingItems, ref currentDisplacement);
                currentDisplacement += new Vector2(0, 20);
            }

            if (completedItems.Count > 0) {
                DrawTitle(b, "Completed", ref currentDisplacement);
                DrawItems(b, completedItems, ref currentDisplacement);
            }
        }

        //Utils.DrawTextWithBox(b, new Vector2(xPositionOnScreen + size.Width/2, yPositionOnScreen + spaceToClearTopBorder), currentTab?.hoverText, Game1.dialogueFont, new Vector2(30, 15), true);
        Utils.DrawTextWithScroll(b, new Vector2(xPositionOnScreen + size.Width / 2, yPositionOnScreen + spaceToClearTopBorder), currentTab?.hoverText, 20);

        drawMouse(b);
        drawHoverText(b, hoverText, Game1.smallFont);
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true) {
        base.receiveLeftClick (x, y, playSound);

        for (var i = 0; i < ModEntry.modData.tabs.Count; i++) {
            var tab = ModEntry.modData.tabs[i];

            if (tab.containsPoint(x, y)) {
                if (playSound) Game1.playSound("smallSelect");

                SetActiveTab(tab);

                return;
            }
        }

        if(createTabButton.containsPoint(x, y)) {
            var menu = new TabCreationMenu(onCreateNewTab);
            menu.open();
            return;
        }

        if (currentTab == null) return;
        foreach (TodoItem item in currentTab.items.ToList()) {
            item.receiveLeftClick(x, y);
        }
    }

    public override void performHoverAction(int x, int y) {
        hoverText = "";

        foreach (var tab in ModEntry.modData.tabs) {
            if (tab.containsPoint(x, y)) {
                hoverText = tab.hoverText;
            }
        }
        
        if(createTabButton.containsPoint(x, y)) {
            hoverText = createTabButton.hoverText;
        }

        if (currentTab == null) return;

        foreach(var item in currentTab.items) {
            item.tryHover(x, y);
        }
    }
    #endregion

    #region Drawing
    private void DrawTitle(SpriteBatch b, string title, ref Vector2 currentDisplacement) {
        float titleHeight = Game1.dialogueFont.MeasureString(title).Y;

        Utility.drawTextWithShadow(b, title, Game1.dialogueFont, itemsAnchorPoint.ToVector2() + currentDisplacement, Game1.textColor);
        currentDisplacement += new Vector2(0, titleHeight);
    }

    private void DrawItems(SpriteBatch b, List<TodoItem> items, ref Vector2 currentDisplacement) {
        for (int i = 0; i < items.Count; i++) {
            TodoItem item = items[i];   

            Vector2 pos = itemsAnchorPoint.ToVector2() + currentDisplacement;

            item.updatePosition(pos);
            item.draw(b);

            currentDisplacement += new Vector2(0, item.textSize.Y);
        }
    }
    #endregion

    #region Events
    private void onCreateNewTab(string name, TabIconNames iconName) {
        var tab = new TodoTab(GetNewTabPosition(), name, iconName);
        ModEntry.modData.tabs.Add(tab);

        SetActiveTab(tab);

        createTabButton.setPosition(GetNewTabPosition());
        Game1.activeClickableMenu = ModEntry.menu;

        ListChange.Invoke(this, EventArgs.Empty);
    }
    #endregion

    #region Utils
    private Vector2 GetNewTabPosition() {
        return tabAnchorPoint.ToVector2() + new Vector2(0, TodoTab.tabSize.Y * TodoTab.Scale * ModEntry.modData.tabs.Count);
    }

    private void SetActiveTab(TodoTab tab) {
        if (currentTab != null) currentTab.active = false;
        tab.active = true;
        ModEntry.modData.lastActiveTab = tab.index;
    }
    #endregion
    #endregion
}