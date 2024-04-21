using StardewValley;
using StardewValley.Menus;
using xTile.Dimensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Rectangle = Microsoft.Xna.Framework.Rectangle;
using TodoList.Extensions;

namespace TodoList.UI;

public class TodoMenu : IClickableMenu {
    private static readonly Size size = new(Game1.uiViewport.Width - 300, Game1.uiViewport.Height - 200);
    private static readonly Point tabDisplacementToOrigin = new(-TodoTab.tabSize.X, TodoTab.tabSize.Y);

    private string hoverText;

    public List<TodoTab> tabs = new();

    public int currentTabIndex = 0;
    public TodoTab? currentTab;

    public TodoMenu(bool playOpeningSound = true)
        : base(
            (Game1.uiViewport.Width - size.Width) / 2, 
            (Game1.uiViewport.Height - size.Height) / 2, 
            size.Width, size.Height,
            showUpperRightCloseButton: true
        ) {
        if(playOpeningSound) {
            Game1.playSound("bigSelect");
        }

        float scale = 4f;
        Point tabAnchorPoint = new(xPositionOnScreen + spaceToClearSideBorder + (tabDisplacementToOrigin.X * (int)scale), yPositionOnScreen + spaceToClearTopBorder);

        tabs.Add(new TodoTab(
            "1",
            new Rectangle(tabAnchorPoint + new Point(0, TodoTab.tabSize.Y * tabs.Count).Multiply(scale), TodoTab.tabSize.Multiply(scale)),
            "",
            "Tab 1",
            Game1.mouseCursors,
            new Rectangle(688, 64, 16, 16),
            scale,
            drawShadow: true
        ));
        tabs.Add(new TodoTab(
            "2",
            new Rectangle(tabAnchorPoint + new Point(0, TodoTab.tabSize.Y * tabs.Count).Multiply(scale), TodoTab.tabSize.Multiply(scale)),
            "",
            "Tab 2l",
            Game1.mouseCursors,
            new Rectangle(688, 64, 16, 16),
            scale,
            drawShadow: true
        ));
    }

    #region Methods
    public override void draw(SpriteBatch b) {
        Game1.drawDialogueBox(xPositionOnScreen, yPositionOnScreen, width, height, speaker: false, drawOnlyBox: true);
        upperRightCloseButton?.draw(b);

        foreach (var tab in tabs) {
            tab.draw(b);

            if (tabs[currentTabIndex] == tab) {
                tab.active = true;
                currentTab = tab;
            }

            if (!string.IsNullOrEmpty(hoverText)) {
                drawHoverText(b, hoverText, Game1.smallFont);
            }
        }

        drawMouse(b);
    }

    public override void receiveLeftClick(int x, int y, bool playSound = true) {
        for(var i = 0 ; i < tabs.Count; i++) {
            var tab = tabs[i];

            if(tab.containsPoint(x, y)) {
                if(playSound) Game1.playSound("smallSelect");

                currentTab.active = false;
                tab.active = true;
                currentTabIndex = i;
            }
        }
    }

    public override void performHoverAction(int x, int y) {
        hoverText = "";

        foreach(var tab in tabs) {
            if (tab.containsPoint(x, y)) {
                hoverText = tab.hoverText;
            }
        }
    }
    #endregion
}