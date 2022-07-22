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
    public class AquilaConfigGameConfigSceneWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Aquila.Config.GameConfig.Scene);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 6, 2, 0);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TERRAIN_BLOCK_OFFSET_DISTANCE", Aquila.Config.GameConfig.Scene.TERRAIN_BLOCK_OFFSET_DISTANCE);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FIGHT_SCENE_DEFAULT_X_WIDTH", Aquila.Config.GameConfig.Scene.FIGHT_SCENE_DEFAULT_X_WIDTH);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FIGHT_SCENE_DEFAULT_Y_WIDTH", Aquila.Config.GameConfig.Scene.FIGHT_SCENE_DEFAULT_Y_WIDTH);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FIGHT_SCENE_TERRAIN_COORDINATE_RANGE", Aquila.Config.GameConfig.Scene.FIGHT_SCENE_TERRAIN_COORDINATE_RANGE);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FIGHT_SCENE_TERRAIN_COORDINATE_PRECISION", Aquila.Config.GameConfig.Scene.FIGHT_SCENE_TERRAIN_COORDINATE_PRECISION);
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "MAIN_CAMERA_DEFAULT_EULER", _g_get_MAIN_CAMERA_DEFAULT_EULER);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "MAIN_CAMERA_DEFAULT_POSITION", _g_get_MAIN_CAMERA_DEFAULT_POSITION);
            
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new Aquila.Config.GameConfig.Scene();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Aquila.Config.GameConfig.Scene constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MAIN_CAMERA_DEFAULT_EULER(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.PushUnityEngineVector3(L, Aquila.Config.GameConfig.Scene.MAIN_CAMERA_DEFAULT_EULER);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MAIN_CAMERA_DEFAULT_POSITION(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.PushUnityEngineVector3(L, Aquila.Config.GameConfig.Scene.MAIN_CAMERA_DEFAULT_POSITION);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
