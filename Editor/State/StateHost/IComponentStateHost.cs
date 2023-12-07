using System.Collections.Generic;

namespace AbandonedCrypt.EditorState
{
  public interface IComponentStateHost
  {
    internal IStateHost RootStateHost { get; set; }
    internal List<IStateVar> StateVars { get; }
  }
}