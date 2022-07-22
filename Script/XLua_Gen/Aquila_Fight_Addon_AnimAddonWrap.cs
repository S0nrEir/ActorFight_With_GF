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
    public class AquilaFightAddonAnimAddonWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Aquila.Fight.Addon.AnimAddon);
			Utils.BeginObjectRegister(type, L, translator, 0, 23, 2, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayDieAnim", _m_PlayDieAnim);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayStandAnim", _m_PlayStandAnim);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayRunAnim", _m_PlayRunAnim);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayAttackAnim", _m_PlayAttackAnim);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayAttack1Anim", _m_PlayAttack1Anim);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayAttack2Anim", _m_PlayAttack2Anim);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayAttack3Anim", _m_PlayAttack3Anim);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlaySkill1Anim", _m_PlaySkill1Anim);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlaySkill2Anim", _m_PlaySkill2Anim);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlaySkill3Anim", _m_PlaySkill3Anim);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlaySkill4Anim", _m_PlaySkill4Anim);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlaySkill5Anim", _m_PlaySkill5Anim);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlaySkill6Anim", _m_PlaySkill6Anim);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayKnockBackAnim", _m_PlayKnockBackAnim);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayInteractionAnim", _m_PlayInteractionAnim);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayVertigoAnim", _m_PlayVertigoAnim);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayHitAnim", _m_PlayHitAnim);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnAdd", _m_OnAdd);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Init", _m_Init);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Dispose", _m_Dispose);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Reset", _m_Reset);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Play", _m_Play);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetEnable", _m_SetEnable);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "AddonType", _g_get_AddonType);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurrClipName", _g_get_CurrClipName);
            
			
			
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
					
					var gen_ret = new Aquila.Fight.Addon.AnimAddon();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Aquila.Fight.Addon.AnimAddon constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayDieAnim(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Addon.AnimAddon gen_to_be_invoked = (Aquila.Fight.Addon.AnimAddon)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.PlayDieAnim(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayStandAnim(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Addon.AnimAddon gen_to_be_invoked = (Aquila.Fight.Addon.AnimAddon)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.PlayStandAnim(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayRunAnim(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Addon.AnimAddon gen_to_be_invoked = (Aquila.Fight.Addon.AnimAddon)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.PlayRunAnim(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayAttackAnim(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Addon.AnimAddon gen_to_be_invoked = (Aquila.Fight.Addon.AnimAddon)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.PlayAttackAnim(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayAttack1Anim(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Addon.AnimAddon gen_to_be_invoked = (Aquila.Fight.Addon.AnimAddon)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.PlayAttack1Anim(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayAttack2Anim(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Addon.AnimAddon gen_to_be_invoked = (Aquila.Fight.Addon.AnimAddon)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.PlayAttack2Anim(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayAttack3Anim(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Addon.AnimAddon gen_to_be_invoked = (Aquila.Fight.Addon.AnimAddon)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.PlayAttack3Anim(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlaySkill1Anim(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Addon.AnimAddon gen_to_be_invoked = (Aquila.Fight.Addon.AnimAddon)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.PlaySkill1Anim(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlaySkill2Anim(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Addon.AnimAddon gen_to_be_invoked = (Aquila.Fight.Addon.AnimAddon)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.PlaySkill2Anim(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlaySkill3Anim(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Addon.AnimAddon gen_to_be_invoked = (Aquila.Fight.Addon.AnimAddon)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.PlaySkill3Anim(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlaySkill4Anim(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Addon.AnimAddon gen_to_be_invoked = (Aquila.Fight.Addon.AnimAddon)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.PlaySkill4Anim(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlaySkill5Anim(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Addon.AnimAddon gen_to_be_invoked = (Aquila.Fight.Addon.AnimAddon)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.PlaySkill5Anim(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlaySkill6Anim(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Addon.AnimAddon gen_to_be_invoked = (Aquila.Fight.Addon.AnimAddon)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.PlaySkill6Anim(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayKnockBackAnim(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Addon.AnimAddon gen_to_be_invoked = (Aquila.Fight.Addon.AnimAddon)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.PlayKnockBackAnim(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayInteractionAnim(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Addon.AnimAddon gen_to_be_invoked = (Aquila.Fight.Addon.AnimAddon)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.PlayInteractionAnim(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayVertigoAnim(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Addon.AnimAddon gen_to_be_invoked = (Aquila.Fight.Addon.AnimAddon)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.PlayVertigoAnim(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayHitAnim(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Addon.AnimAddon gen_to_be_invoked = (Aquila.Fight.Addon.AnimAddon)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.PlayHitAnim(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnAdd(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Addon.AnimAddon gen_to_be_invoked = (Aquila.Fight.Addon.AnimAddon)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnAdd(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Init(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Addon.AnimAddon gen_to_be_invoked = (Aquila.Fight.Addon.AnimAddon)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Aquila.Fight.Actor.TActorBase _actor = (Aquila.Fight.Actor.TActorBase)translator.GetObject(L, 2, typeof(Aquila.Fight.Actor.TActorBase));
                    UnityEngine.GameObject _targetGameObject = (UnityEngine.GameObject)translator.GetObject(L, 3, typeof(UnityEngine.GameObject));
                    UnityEngine.Transform _targetTransform = (UnityEngine.Transform)translator.GetObject(L, 4, typeof(UnityEngine.Transform));
                    
                    gen_to_be_invoked.Init( _actor, _targetGameObject, _targetTransform );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Dispose(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Addon.AnimAddon gen_to_be_invoked = (Aquila.Fight.Addon.AnimAddon)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Dispose(  );
                    
                    
                    
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
            
            
                Aquila.Fight.Addon.AnimAddon gen_to_be_invoked = (Aquila.Fight.Addon.AnimAddon)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Reset(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Play(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Addon.AnimAddon gen_to_be_invoked = (Aquila.Fight.Addon.AnimAddon)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _clipName = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.Play( _clipName );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetEnable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Aquila.Fight.Addon.AnimAddon gen_to_be_invoked = (Aquila.Fight.Addon.AnimAddon)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _enable = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.SetEnable( _enable );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AddonType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Aquila.Fight.Addon.AnimAddon gen_to_be_invoked = (Aquila.Fight.Addon.AnimAddon)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.AddonType);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurrClipName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Aquila.Fight.Addon.AnimAddon gen_to_be_invoked = (Aquila.Fight.Addon.AnimAddon)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.CurrClipName);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
