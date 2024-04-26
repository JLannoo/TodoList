using StardewModdingAPI;
using StardewValley;
using TodoList.UI.Menus;

namespace TodoList;

internal class ModEntry : Mod {
    public static IMonitor monitor;
    public static IModHelper modHelper;
    
    public static TodoMenu menu;

    public override void Entry(IModHelper helper) {
        monitor = Monitor;
        modHelper = helper;

        helper.Events.Input.ButtonPressed += OnButtonPressed;
    }

    #region Events
    private void OnButtonPressed(object? sender, StardewModdingAPI.Events.ButtonPressedEventArgs e) {
        if (e.Button == SButton.L) {
            if (Game1.activeClickableMenu is TodoMenu) {
                menu.exitThisMenu();
            } else {
                menu = new TodoMenu();
                Game1.activeClickableMenu = menu;
            }
        }
    }
    #endregion
}
