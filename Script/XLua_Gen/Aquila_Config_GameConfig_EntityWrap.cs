#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class AquilaConfigGameConfigEntityWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Aquila.Config.GameConfig.Entity);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 8, 0, 0);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GROUP_HeroActor", Aquila.Config.GameConfig.Entity.GROUP_HeroActor);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GROUP_ActorEffect", Aquila.Config.GameConfig.Entity.GROUP_ActorEffect);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GROUP_Projectile", Aquila.Config.GameConfig.Entity.GROUP_Projectile);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GROUP_Trigger", Aquila.Config.GameConfig.Entity.GROUP_Trigger);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GROUP_Other", Aquila.Config.GameConfig.Entity.GROUP_Other);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Priority_Actor", Aquila.Config.GameConfig.Entity.Priority_Actor);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Priority_Effect", Aquila.Config.GameConfig.Entity.Priority_Effect);
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Aquila.Config.GameConfig.Entity does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        
        
        
        
        
		
		
		
		
    }
}
