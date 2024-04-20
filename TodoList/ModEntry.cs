using StardewModdingAPI;

namespace TodoList;

internal class ModEntry : Mod {
    public static IMonitor monitor;

    public override void Entry(IModHelper helper) {
        monitor = Monitor;
    }
}
