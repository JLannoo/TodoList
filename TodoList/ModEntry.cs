using StardewModdingAPI;
using StardewValley;
using System.Text.Json.Serialization;
using TodoList.UI.Menus;

namespace TodoList;

internal class ModEntry : Mod {
    public static IMonitor monitor;
    public static IModHelper modHelper;
    
    public static ModData? modData;
    public static TodoMenu? menu;

    public override void Entry(IModHelper helper) {
        monitor = Monitor;
        modHelper = helper;

        helper.Events.Input.ButtonPressed += OnButtonPressed;
        helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
    }


    #region Events
    private void OnButtonPressed(object? sender, StardewModdingAPI.Events.ButtonPressedEventArgs e) {
        if (Context.IsWorldReady && e.Button == SButton.L) {
            if (Game1.activeClickableMenu is TodoMenu) {
                menu?.exitThisMenu();
                menu.ListChange -= OnListChange;
            } else {
                menu = new TodoMenu();
                menu.ListChange += OnListChange;
                Game1.activeClickableMenu = menu;
            }
        }
    }

    #region Event Handlers
    private void OnSaveLoaded(object? sender, StardewModdingAPI.Events.SaveLoadedEventArgs e) {
        LoadSaveData();
    }

    private void OnListChange(object? sender, EventArgs e) {
        WriteSavedata();
    }
    #endregion

    #region SaveData Methods
    private ModData LoadSaveData() {
        var rawData = modHelper.Data.ReadJsonFile<SerializedModData>(ModData.key);

        var data = rawData != null ? ModDataSerializer.deserialize(rawData) : new ModData();

        monitor.Log($"Loaded save data: {data.tabs.Count} tabs - {data.tabs.Aggregate(0, (acc, tab) => acc+=tab.items.Count)} items", LogLevel.Info);
        modData = data;
        return data;

    }

    private void WriteSavedata() {
        modHelper.Data.WriteJsonFile(ModData.key, ModDataSerializer.serialize(modData));
    }
    #endregion
    #endregion
}
