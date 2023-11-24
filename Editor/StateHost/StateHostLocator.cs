using System.Collections.Generic;

namespace AbandonedCrypt.EditorState
{
  public static class StateHostLocator
  {
    private static readonly Dictionary<string, IStateHost> stateHosts = new();

    internal static void Register(IStateHost stateHost)
    {
      if (stateHosts.ContainsKey(stateHost.GetType().Name))
        throw new StateHostRegisteredException($"A StateHost with the name '{stateHost.GetType().Name}' has already been registered. StateHost names are bound to their Editor's name. Please use unique Editor names.");
      stateHosts.Add(stateHost.GetType().Name, stateHost);
    }

    internal static void Unregister(IStateHost stateHost)
    {
      if (!stateHosts.ContainsKey(stateHost.GetType().Name))
        throw new StateHostNotFoundException($"A StateHost with the name '{stateHost.GetType().Name}' has not been registered.");
      stateHosts.Remove(stateHost.GetType().Name);
    }

    public static IStateHost Find(string name)
    {
      if (!stateHosts.ContainsKey(name))
        throw new StateHostNotFoundException($"A StateHost with the name '{name}' has not been registered.");
      return stateHosts[name];
    }
  }
}