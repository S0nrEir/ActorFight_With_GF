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
    public class AquilaFightActorTActorBaseWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Aquila.Fight.Actor.TActorBase);
			Utils.BeginObjectRegister(type, L, translator, 0, 15, 4, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Trigger", _m_Trigger);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Valid", _m_Valid);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RegisterActorEvent", _m_RegisterActorEvent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UnRegisterActorEvent", _m_UnRegisterActorEvent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsMine", _m_IsMine);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetLayer", _m_SetLayer);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetActorID", _m_SetActorID);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetQuaternion", _m_SetQuaternion);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetLocalPosition", _m_SetLocalPosition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetWorldPosition", _m_SetWorldPosition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetTag", _m_SetTag);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetHostID", _m_SetHostID);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Setup", _m_Setup);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetDataID", _m_SetDataID);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Reset", _m_Reset);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "ActorType", _g_get_ActorType);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ActorID", _g_get_ActorID);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AddonInitFlag", _g_get_AddonInitFlag);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "HostID", _g_get_HostID);
            
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Aquila.Fight.Actor.TActorBase does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Trigger(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Actor.TActorBase gen_to_be_invoked = (Aquila.Fight.Actor.TActorBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Aquila.Fight.Addon.ActorEventEnum _type;translator.Get(L, 2, out _type);
                    object[] _param = translator.GetParams<object>(L, 3);
                    
                    gen_to_be_invoked.Trigger( _type, _param );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Valid(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Actor.TActorBase gen_to_be_invoked = (Aquila.Fight.Actor.TActorBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.Valid(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RegisterActorEvent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Actor.TActorBase gen_to_be_invoked = (Aquila.Fight.Actor.TActorBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Aquila.Fight.Addon.ActorEventEnum _type;translator.Get(L, 2, out _type);
                    System.Action<int, object[]> _action = translator.GetDelegate<System.Action<int, object[]>>(L, 3);
                    
                    gen_to_be_invoked.RegisterActorEvent( _type, _action );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnRegisterActorEvent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Actor.TActorBase gen_to_be_invoked = (Aquila.Fight.Actor.TActorBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Aquila.Fight.Addon.ActorEventEnum _type;translator.Get(L, 2, out _type);
                    
                    gen_to_be_invoked.UnRegisterActorEvent( _type );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsMine(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Actor.TActorBase gen_to_be_invoked = (Aquila.Fight.Actor.TActorBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.IsMine(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetLayer(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Actor.TActorBase gen_to_be_invoked = (Aquila.Fight.Actor.TActorBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.SetLayer(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetActorID(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Actor.TActorBase gen_to_be_invoked = (Aquila.Fight.Actor.TActorBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.SetActorID( _id );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetQuaternion(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Actor.TActorBase gen_to_be_invoked = (Aquila.Fight.Actor.TActorBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Quaternion _rotationToSet;translator.Get(L, 2, out _rotationToSet);
                    
                    gen_to_be_invoked.SetQuaternion( _rotationToSet );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetLocalPosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Actor.TActorBase gen_to_be_invoked = (Aquila.Fight.Actor.TActorBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _posToSet;translator.Get(L, 2, out _posToSet);
                    
                    gen_to_be_invoked.SetLocalPosition( _posToSet );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetWorldPosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Actor.TActorBase gen_to_be_invoked = (Aquila.Fight.Actor.TActorBase)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Vector3>(L, 2)) 
                {
                    UnityEngine.Vector3 _posToSet;translator.Get(L, 2, out _posToSet);
                    
                    gen_to_be_invoked.SetWorldPosition( _posToSet );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Vector2>(L, 2)) 
                {
                    UnityEngine.Vector2 _posToSet;translator.Get(L, 2, out _posToSet);
                    
                    gen_to_be_invoked.SetWorldPosition( _posToSet );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Aquila.Fight.Actor.TActorBase.SetWorldPosition!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetTag(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Actor.TActorBase gen_to_be_invoked = (Aquila.Fight.Actor.TActorBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _tag = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.SetTag( _tag );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetHostID(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Actor.TActorBase gen_to_be_invoked = (Aquila.Fight.Actor.TActorBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    ulong _hostID = LuaAPI.lua_touint64(L, 2);
                    
                    gen_to_be_invoked.SetHostID( _hostID );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Setup(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Actor.TActorBase gen_to_be_invoked = (Aquila.Fight.Actor.TActorBase)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& (LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5) || LuaAPI.lua_isuint64(L, 5))) 
                {
                    string _tag = LuaAPI.lua_tostring(L, 2);
                    int _index = LuaAPI.xlua_tointeger(L, 3);
                    int _actorID = LuaAPI.xlua_tointeger(L, 4);
                    ulong _hostID = LuaAPI.lua_touint64(L, 5);
                    
                    gen_to_be_invoked.Setup( _tag, _index, _actorID, _hostID );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 6&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& (LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5) || LuaAPI.lua_isuint64(L, 5))&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    string _tag = LuaAPI.lua_tostring(L, 2);
                    int _index = LuaAPI.xlua_tointeger(L, 3);
                    int _actorID = LuaAPI.xlua_tointeger(L, 4);
                    ulong _hostID = LuaAPI.lua_touint64(L, 5);
                    int _forceType = LuaAPI.xlua_tointeger(L, 6);
                    
                    gen_to_be_invoked.Setup( _tag, _index, _actorID, _hostID, _forceType );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& (LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5) || LuaAPI.lua_isuint64(L, 5))) 
                {
                    string _tag = LuaAPI.lua_tostring(L, 2);
                    int _index = LuaAPI.xlua_tointeger(L, 3);
                    int _actorID = LuaAPI.xlua_tointeger(L, 4);
                    ulong _hostID = LuaAPI.lua_touint64(L, 5);
                    
                    gen_to_be_invoked.Setup( _tag, _index, _actorID, _hostID );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 7&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& (LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5) || LuaAPI.lua_isuint64(L, 5))&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)) 
                {
                    string _tag = LuaAPI.lua_tostring(L, 2);
                    int _index = LuaAPI.xlua_tointeger(L, 3);
                    int _actorID = LuaAPI.xlua_tointeger(L, 4);
                    ulong _hostID = LuaAPI.lua_touint64(L, 5);
                    int _forceType = LuaAPI.xlua_tointeger(L, 6);
                    int _dataID = LuaAPI.xlua_tointeger(L, 7);
                    
                    gen_to_be_invoked.Setup( _tag, _index, _actorID, _hostID, _forceType, _dataID );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Aquila.Fight.Actor.TActorBase.Setup!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetDataID(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Actor.TActorBase gen_to_be_invoked = (Aquila.Fight.Actor.TActorBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _roleBaseID = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.SetDataID( _roleBaseID );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Reset(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Actor.TActorBase gen_to_be_invoked = (Aquila.Fight.Actor.TActorBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Reset(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ActorType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Aquila.Fight.Actor.TActorBase gen_to_be_invoked = (Aquila.Fight.Actor.TActorBase)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ActorType);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ActorID(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Aquila.Fight.Actor.TActorBase gen_to_be_invoked = (Aquila.Fight.Actor.TActorBase)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.ActorID);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AddonInitFlag(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Aquila.Fight.Actor.TActorBase gen_to_be_invoked = (Aquila.Fight.Actor.TActorBase)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.AddonInitFlag);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_HostID(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Aquila.Fight.Actor.TActorBase gen_to_be_invoked = (Aquila.Fight.Actor.TActorBase)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushuint64(L, gen_to_be_invoked.HostID);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
