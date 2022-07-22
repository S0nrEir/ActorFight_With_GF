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
    public class AquilaConfigGameConfigLuaWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Aquila.Config.GameConfig.Lua);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 5, 0, 0);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LUA_FUNCTION_NAME_ON_FINISH", Aquila.Config.GameConfig.Lua.LUA_FUNCTION_NAME_ON_FINISH);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LUA_FUNCTION_NAME_ON_TICK", Aquila.Config.GameConfig.Lua.LUA_FUNCTION_NAME_ON_TICK);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LUA_FUNCTION_NAME_ON_UPDATE", Aquila.Config.GameConfig.Lua.LUA_FUNCTION_NAME_ON_UPDATE);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LUA_FUNCTION_NAME_ON_START", Aquila.Config.GameConfig.Lua.LUA_FUNCTION_NAME_ON_START);
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Aquila.Config.GameConfig.Lua does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        
        
        
        
        
		
		
		
		
    }
}
