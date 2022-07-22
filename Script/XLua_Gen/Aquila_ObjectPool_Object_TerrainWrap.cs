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
    public class AquilaObjectPoolObject_TerrainWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Aquila.ObjectPool.Object_Terrain);
			Utils.BeginObjectRegister(type, L, translator, 0, 6, 2, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetNameTest", _m_SetNameTest);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetName", _m_SetName);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetParent", _m_SetParent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetPosition", _m_SetPosition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetCoordinate", _m_SetCoordinate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetHeight", _m_SetHeight);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Coordinate", _g_get_Coordinate);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UniqueKey", _g_get_UniqueKey);
            
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "Gen", _m_Gen_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new Aquila.ObjectPool.Object_Terrain();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Aquila.ObjectPool.Object_Terrain constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetNameTest(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.ObjectPool.Object_Terrain gen_to_be_invoked = (Aquila.ObjectPool.Object_Terrain)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _name = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.SetNameTest( _name );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetName(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.ObjectPool.Object_Terrain gen_to_be_invoked = (Aquila.ObjectPool.Object_Terrain)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _name = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.SetName( _name );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetParent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.ObjectPool.Object_Terrain gen_to_be_invoked = (Aquila.ObjectPool.Object_Terrain)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
                    
                    gen_to_be_invoked.SetParent( _parent );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetPosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.ObjectPool.Object_Terrain gen_to_be_invoked = (Aquila.ObjectPool.Object_Terrain)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _z = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.SetPosition( _x, _z );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetCoordinate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.ObjectPool.Object_Terrain gen_to_be_invoked = (Aquila.ObjectPool.Object_Terrain)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.SetCoordinate( _x, _y );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetHeight(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.ObjectPool.Object_Terrain gen_to_be_invoked = (Aquila.ObjectPool.Object_Terrain)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _height = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.SetHeight( _height );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Gen_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    
                        var gen_ret = Aquila.ObjectPool.Object_Terrain.Gen( _go );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Coordinate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Aquila.ObjectPool.Object_Terrain gen_to_be_invoked = (Aquila.ObjectPool.Object_Terrain)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Coordinate);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UniqueKey(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Aquila.ObjectPool.Object_Terrain gen_to_be_invoked = (Aquila.ObjectPool.Object_Terrain)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.UniqueKey);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
