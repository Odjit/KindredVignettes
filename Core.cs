using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ProjectM.Network;
using ProjectM.Scripting;
using Unity.Entities;

namespace KindredVignettes;

internal static class Core
{
	public static World Server { get; } = GetWorld("Server") ?? throw new System.Exception("There is no Server world (yet). Did you install a server mod on the client?");

	public static EntityManager EntityManager { get; } = Server.EntityManager;

	static NetworkIdSystem networkIdSystem;

    public static NetworkIdSystem NetworkIdSystem { get
		{
			if (networkIdSystem == null)
			{
                networkIdSystem = Server.GetExistingSystem<NetworkIdSystem>();
            }
			return networkIdSystem;
		}}

	static ServerScriptMapper serverScriptMapper;
	public static ServerScriptMapper ServerScriptMapper { get
		{
			if (serverScriptMapper == null)
			{
                serverScriptMapper = Server.GetExistingSystem<ServerScriptMapper>();
            }
			return serverScriptMapper;
		}
	}

    public static ManualLogSource Log { get; } = Plugin.PluginLog;

	public static void LogException(System.Exception e, [CallerMemberName] string caller = null)
	{
		Core.Log.LogError($"Failure in {caller}\nMessage: {e.Message} Inner:{e.InnerException?.Message}\n\nStack: {e.StackTrace}\nInner Stack: {e.InnerException?.StackTrace}");
	}


	internal static void InitializeAfterLoaded()
	{
		if (_hasInitialized) return;

		_hasInitialized = true;

        Log.LogInfo($"{nameof(InitializeAfterLoaded)} completed");
	}
	private static bool _hasInitialized = false;

	private static World GetWorld(string name)
	{
		foreach (var world in World.s_AllWorlds)
		{
			if (world.Name == name)
			{
				return world;
			}
		}

		return null;
	}
}