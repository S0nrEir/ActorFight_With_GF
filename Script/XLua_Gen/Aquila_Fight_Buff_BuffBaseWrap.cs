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
    public class AquilaFightBuffBuffBaseWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Aquila.Fight.Buff.BuffBase);
			Utils.BeginObjectRegister(type, L, translator, 0, 9, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Setup", _m_Setup);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ApplyCache", _m_ApplyCache);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Cache", _m_Cache);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Get", _m_Get);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Remove", _m_Remove);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Add", _m_Add);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreateEffect", _m_CreateEffect);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Contains", _m_Contains);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Clear", _m_Clear);
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new Aquila.Fight.Buff.BuffBase();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Aquila.Fight.Buff.BuffBase constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Setup(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Buff.BuffBase gen_to_be_invoked = (Aquila.Fight.Buff.BuffBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.Setup( _id );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ApplyCache(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Buff.BuffBase gen_to_be_invoked = (Aquila.Fight.Buff.BuffBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Aquila.Fight.Actor.TActorBase _actor = (Aquila.Fight.Actor.TActorBase)translator.GetObject(L, 2, typeof(Aquila.Fight.Actor.TActorBase));
                    
                    gen_to_be_invoked.ApplyCache( _actor );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Cache(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Buff.BuffBase gen_to_be_invoked = (Aquila.Fight.Buff.BuffBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _objID = LuaAPI.xlua_tointeger(L, 2);
                    Aquila.Fight.Buff.BuffEntity _entity = (Aquila.Fight.Buff.BuffEntity)translator.GetObject(L, 3, typeof(Aquila.Fight.Buff.BuffEntity));
                    
                    gen_to_be_invoked.Cache( _objID, _entity );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Get(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Buff.BuffBase gen_to_be_invoked = (Aquila.Fight.Buff.BuffBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _objID = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.Get( _objID );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Remove(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Buff.BuffBase gen_to_be_invoked = (Aquila.Fight.Buff.BuffBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _objID = LuaAPI.xlua_tointeger(L, 2);
                    bool _emptyEntity;
                    
                        var gen_ret = gen_to_be_invoked.Remove( _objID, out _emptyEntity );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    LuaAPI.lua_pushboolean(L, _emptyEntity);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Add(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Buff.BuffBase gen_to_be_invoked = (Aquila.Fight.Buff.BuffBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _objID = LuaAPI.xlua_tointeger(L, 2);
                    int _impactID = LuaAPI.xlua_tointeger(L, 3);
                    bool _createSucc;
                    
                        var gen_ret = gen_to_be_invoked.Add( _objID, _impactID, out _createSucc );
                        translator.Push(L, gen_ret);
                    LuaAPI.lua_pushboolean(L, _createSucc);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateEffect(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Buff.BuffBase gen_to_be_invoked = (Aquila.Fight.Buff.BuffBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Aquila.Fight.Actor.TActorBase _actor = (Aquila.Fight.Actor.TActorBase)translator.GetObject(L, 2, typeof(Aquila.Fight.Actor.TActorBase));
                    Aquila.Fight.Buff.BuffEntity _entity = (Aquila.Fight.Buff.BuffEntity)translator.GetObject(L, 3, typeof(Aquila.Fight.Buff.BuffEntity));
                    
                    gen_to_be_invoked.CreateEffect( _actor, _entity );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Contains(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Buff.BuffBase gen_to_be_invoked = (Aquila.Fight.Buff.BuffBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _objID = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.Contains( _objID );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Clear(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Buff.BuffBase gen_to_be_invoked = (Aquila.Fight.Buff.BuffBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Clear(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
