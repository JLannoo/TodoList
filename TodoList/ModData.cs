using Microsoft.Xna.Framework;
using StardewModdingAPI;
using TodoList.UI.Components;
using TodoList.UI.Menus;

namespace TodoList;

public class ModData {
    public static readonly string key = $"data/{Constants.SaveFolderName}.json";

    public List<TodoTab> tabs { get; set; } = new List<TodoTab>();
    public int lastActiveTab;

    public ModData() { }
    public ModData(List<TodoTab> tabs) {
        this.tabs = tabs;

        lastActiveTab = tabs.Find(tab => tab.active)?.index ?? 0;
    }
}

public class SerializedItem {
    public string text;
    public bool completed;

    public SerializedItem(string text, bool completed) {
        this.text = text;
        this.completed = completed;
    }
}

public class SerializedTab {
    public Vector2 position;
    public int index;
    public string name;
    public bool active;
    public List<SerializedItem> items = new();
    public TabIconNames iconName;

    public SerializedTab() { }

    public SerializedTab(TodoTab tab) {
        position = tab.bounds.Location.ToVector2();
        index = tab.index;
        active = tab.active;
        iconName = tab.iconName;
        name = tab.hoverText;
    }
}

public class SerializedModData {
    public List<SerializedTab> tabs;

    public SerializedModData(List<SerializedTab> tabs) {
        this.tabs = tabs;
    }
}

public static class ModDataSerializer {
    public static SerializedModData serialize(ModData data) {
        List<SerializedTab> tabs = new();

        foreach(var tab in data.tabs) {
            var serializedTab = new SerializedTab(tab);

            foreach (var item in tab.items) {
                var serializedItem = new SerializedItem(item.text, item.completed);
                serializedTab.items.Add(serializedItem);
            }

            tabs.Add(serializedTab);
        }

        return new SerializedModData(tabs);
    }

    public static ModData deserialize(SerializedModData data) {
        List<TodoTab> tabs = new();

        foreach (var tab in data.tabs) {
            var deserializedTab = new TodoTab(tab);

            foreach(var item in tab.items) {
                deserializedTab.items.Add(new TodoItem(deserializedTab, item.text));
            }

            tabs.Add(deserializedTab);
        }

        return new ModData(tabs);
    }
}