using System.Collections.Generic;

namespace AbandonedCrypt.EditorState
{
  public static class StateContext
  {
    private static readonly Dictionary<string, StateRepository> repositories = new();

    internal static void Register(string name, StateRepository repo)
    {
      if (repositories.ContainsKey(name))
        throw new RepositoryRegisteredException($"A StateRepository with the name '{name}' has already been registered. Repository names are bound to their Editor's name. Please use unique Editor names.");
      repositories.Add(name, repo);
    }

    internal static void Unregister(string name)
    {
      if (!repositories.ContainsKey(name))
        throw new RepositoryNotFoundException($"A StateRepository with the name '{name}' has not been registered.");
      repositories.Remove(name);
    }

    public static StateRepository Find(string name)
    {
      if (!repositories.ContainsKey(name))
        throw new RepositoryNotFoundException($"A StateRepository with the name '{name}' has not been registered.");
      return repositories[name];
    }
  }
}