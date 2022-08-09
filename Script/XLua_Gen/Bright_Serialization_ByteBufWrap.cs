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
using Bright.Serialization;

namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class BrightSerializationByteBufWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Bright.Serialization.ByteBuf);
			Utils.BeginObjectRegister(type, L, translator, 0, 81, 9, 2);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Replace", _m_Replace);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddWriteIndex", _m_AddWriteIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddReadIndex", _m_AddReadIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CopyData", _m_CopyData);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DiscardReadBytes", _m_DiscardReadBytes);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteBytesWithoutSize", _m_WriteBytesWithoutSize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Clear", _m_Clear);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "EnsureWrite", _m_EnsureWrite);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Append", _m_Append);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteBool", _m_WriteBool);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadBool", _m_ReadBool);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteByte", _m_WriteByte);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadByte", _m_ReadByte);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteShort", _m_WriteShort);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadShort", _m_ReadShort);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadFshort", _m_ReadFshort);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteFshort", _m_WriteFshort);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteInt", _m_WriteInt);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadInt", _m_ReadInt);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteUint", _m_WriteUint);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadUint", _m_ReadUint);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteUint_Unsafe", _m_WriteUint_Unsafe);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadUint_Unsafe", _m_ReadUint_Unsafe);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadFint", _m_ReadFint);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteFint", _m_WriteFint);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadFint_Safe", _m_ReadFint_Safe);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteFint_Safe", _m_WriteFint_Safe);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteLong", _m_WriteLong);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadLong", _m_ReadLong);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteNumberAsLong", _m_WriteNumberAsLong);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadLongAsNumber", _m_ReadLongAsNumber);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadUlong", _m_ReadUlong);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteFlong", _m_WriteFlong);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadFlong", _m_ReadFlong);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteFloat", _m_WriteFloat);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadFloat", _m_ReadFloat);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteDouble", _m_WriteDouble);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadDouble", _m_ReadDouble);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteSize", _m_WriteSize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadSize", _m_ReadSize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteSint", _m_WriteSint);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadSint", _m_ReadSint);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteSlong", _m_WriteSlong);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadSlong", _m_ReadSlong);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteString", _m_WriteString);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadString", _m_ReadString);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteBytes", _m_WriteBytes);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadBytes", _m_ReadBytes);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteComplex", _m_WriteComplex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadComplex", _m_ReadComplex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteVector2", _m_WriteVector2);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadVector2", _m_ReadVector2);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteVector3", _m_WriteVector3);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadVector3", _m_ReadVector3);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteVector4", _m_WriteVector4);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadVector4", _m_ReadVector4);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteQuaternion", _m_WriteQuaternion);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadQuaternion", _m_ReadQuaternion);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteMatrix4x4", _m_WriteMatrix4x4);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadMatrix4x4", _m_ReadMatrix4x4);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteByteBufWithSize", _m_WriteByteBufWithSize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteByteBufWithoutSize", _m_WriteByteBufWithoutSize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "TryReadByte", _m_TryReadByte);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "TryDeserializeInplaceByteBuf", _m_TryDeserializeInplaceByteBuf);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteRawTag", _m_WriteRawTag);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "BeginWriteSegment", _m_BeginWriteSegment);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "EndWriteSegment", _m_EndWriteSegment);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadSegment", _m_ReadSegment);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "EnterSegment", _m_EnterSegment);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LeaveSegment", _m_LeaveSegment);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ToString", _m_ToString);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Equals", _m_Equals);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Clone", _m_Clone);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetHashCode", _m_GetHashCode);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Release", _m_Release);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteUnityVector2", _m_WriteUnityVector2);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadUnityVector2", _m_ReadUnityVector2);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteUnityVector3", _m_WriteUnityVector3);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadUnityVector3", _m_ReadUnityVector3);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteUnityVector4", _m_WriteUnityVector4);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadUnityVector4", _m_ReadUnityVector4);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "ReaderIndex", _g_get_ReaderIndex);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "WriterIndex", _g_get_WriterIndex);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Capacity", _g_get_Capacity);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Size", _g_get_Size);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Empty", _g_get_Empty);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "NotEmpty", _g_get_NotEmpty);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Bytes", _g_get_Bytes);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Remaining", _g_get_Remaining);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "NotCompactWritable", _g_get_NotCompactWritable);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "ReaderIndex", _s_set_ReaderIndex);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "WriterIndex", _s_set_WriterIndex);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 3, 1, 1);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "Wrap", _m_Wrap_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "FromString", _m_FromString_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "StringCacheFinder", _g_get_StringCacheFinder);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "StringCacheFinder", _s_set_StringCacheFinder);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new Bright.Serialization.ByteBuf();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) == 2 && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2))
				{
					int _capacity = LuaAPI.xlua_tointeger(L, 2);
					
					var gen_ret = new Bright.Serialization.ByteBuf(_capacity);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) == 2 && (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
				{
					byte[] _bytes = LuaAPI.lua_tobytes(L, 2);
					
					var gen_ret = new Bright.Serialization.ByteBuf(_bytes);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) == 4 && (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4))
				{
					byte[] _bytes = LuaAPI.lua_tobytes(L, 2);
					int _readIndex = LuaAPI.xlua_tointeger(L, 3);
					int _writeIndex = LuaAPI.xlua_tointeger(L, 4);
					
					var gen_ret = new Bright.Serialization.ByteBuf(_bytes, _readIndex, _writeIndex);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) == 3 && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2) && translator.Assignable<System.Action<Bright.Serialization.ByteBuf>>(L, 3))
				{
					int _capacity = LuaAPI.xlua_tointeger(L, 2);
					System.Action<Bright.Serialization.ByteBuf> _releaser = translator.GetDelegate<System.Action<Bright.Serialization.ByteBuf>>(L, 3);
					
					var gen_ret = new Bright.Serialization.ByteBuf(_capacity, _releaser);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Bright.Serialization.ByteBuf constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Wrap_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    byte[] _bytes = LuaAPI.lua_tobytes(L, 1);
                    
                        var gen_ret = Bright.Serialization.ByteBuf.Wrap( _bytes );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Replace(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    byte[] _bytes = LuaAPI.lua_tobytes(L, 2);
                    
                    gen_to_be_invoked.Replace( _bytes );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    byte[] _bytes = LuaAPI.lua_tobytes(L, 2);
                    int _beginPos = LuaAPI.xlua_tointeger(L, 3);
                    int _endPos = LuaAPI.xlua_tointeger(L, 4);
                    
                    gen_to_be_invoked.Replace( _bytes, _beginPos, _endPos );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Bright.Serialization.ByteBuf.Replace!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddWriteIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _add = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.AddWriteIndex( _add );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddReadIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _add = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.AddReadIndex( _add );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CopyData(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.CopyData(  );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DiscardReadBytes(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.DiscardReadBytes(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteBytesWithoutSize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    byte[] _bs = LuaAPI.lua_tobytes(L, 2);
                    
                    gen_to_be_invoked.WriteBytesWithoutSize( _bs );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    byte[] _bs = LuaAPI.lua_tobytes(L, 2);
                    int _offset = LuaAPI.xlua_tointeger(L, 3);
                    int _len = LuaAPI.xlua_tointeger(L, 4);
                    
                    gen_to_be_invoked.WriteBytesWithoutSize( _bs, _offset, _len );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Bright.Serialization.ByteBuf.WriteBytesWithoutSize!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Clear(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Clear(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_EnsureWrite(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _size = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.EnsureWrite( _size );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Append(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    byte _x = (byte)LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.Append( _x );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteBool(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _b = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.WriteBool( _b );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadBool(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ReadBool(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteByte(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    byte _x = (byte)LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.WriteByte( _x );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadByte(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ReadByte(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteShort(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    short _x = (short)LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.WriteShort( _x );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadShort(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ReadShort(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadFshort(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ReadFshort(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteFshort(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    short _x = (short)LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.WriteFshort( _x );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteInt(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.WriteInt( _x );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadInt(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ReadInt(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteUint(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    uint _x = LuaAPI.xlua_touint(L, 2);
                    
                    gen_to_be_invoked.WriteUint( _x );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadUint(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ReadUint(  );
                        LuaAPI.xlua_pushuint(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteUint_Unsafe(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    uint _x = LuaAPI.xlua_touint(L, 2);
                    
                    gen_to_be_invoked.WriteUint_Unsafe( _x );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadUint_Unsafe(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ReadUint_Unsafe(  );
                        LuaAPI.xlua_pushuint(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadFint(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ReadFint(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteFint(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.WriteFint( _x );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadFint_Safe(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ReadFint_Safe(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteFint_Safe(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.WriteFint_Safe( _x );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteLong(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    long _x = LuaAPI.lua_toint64(L, 2);
                    
                    gen_to_be_invoked.WriteLong( _x );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadLong(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ReadLong(  );
                        LuaAPI.lua_pushint64(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteNumberAsLong(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    double _x = LuaAPI.lua_tonumber(L, 2);
                    
                    gen_to_be_invoked.WriteNumberAsLong( _x );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadLongAsNumber(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ReadLongAsNumber(  );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadUlong(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ReadUlong(  );
                        LuaAPI.lua_pushuint64(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteFlong(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    long _x = LuaAPI.lua_toint64(L, 2);
                    
                    gen_to_be_invoked.WriteFlong( _x );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadFlong(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ReadFlong(  );
                        LuaAPI.lua_pushint64(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteFloat(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    gen_to_be_invoked.WriteFloat( _x );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadFloat(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ReadFloat(  );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteDouble(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    double _x = LuaAPI.lua_tonumber(L, 2);
                    
                    gen_to_be_invoked.WriteDouble( _x );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadDouble(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ReadDouble(  );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteSize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _n = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.WriteSize( _n );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadSize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ReadSize(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteSint(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.WriteSint( _x );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadSint(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ReadSint(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteSlong(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    long _x = LuaAPI.lua_toint64(L, 2);
                    
                    gen_to_be_invoked.WriteSlong( _x );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadSlong(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ReadSlong(  );
                        LuaAPI.lua_pushint64(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteString(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _x = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.WriteString( _x );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadString(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ReadString(  );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteBytes(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    byte[] _x = LuaAPI.lua_tobytes(L, 2);
                    
                    gen_to_be_invoked.WriteBytes( _x );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadBytes(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ReadBytes(  );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteComplex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Numerics.Complex _x;translator.Get(L, 2, out _x);
                    
                    gen_to_be_invoked.WriteComplex( _x );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadComplex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ReadComplex(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteVector2(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Numerics.Vector2 _x;translator.Get(L, 2, out _x);
                    
                    gen_to_be_invoked.WriteVector2( _x );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadVector2(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ReadVector2(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteVector3(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Numerics.Vector3 _x;translator.Get(L, 2, out _x);
                    
                    gen_to_be_invoked.WriteVector3( _x );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadVector3(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ReadVector3(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteVector4(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Numerics.Vector4 _x;translator.Get(L, 2, out _x);
                    
                    gen_to_be_invoked.WriteVector4( _x );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadVector4(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ReadVector4(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteQuaternion(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Numerics.Quaternion _x;translator.Get(L, 2, out _x);
                    
                    gen_to_be_invoked.WriteQuaternion( _x );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadQuaternion(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ReadQuaternion(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteMatrix4x4(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Numerics.Matrix4x4 _x;translator.Get(L, 2, out _x);
                    
                    gen_to_be_invoked.WriteMatrix4x4( _x );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadMatrix4x4(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ReadMatrix4x4(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteByteBufWithSize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Bright.Serialization.ByteBuf _o = (Bright.Serialization.ByteBuf)translator.GetObject(L, 2, typeof(Bright.Serialization.ByteBuf));
                    
                    gen_to_be_invoked.WriteByteBufWithSize( _o );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteByteBufWithoutSize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Bright.Serialization.ByteBuf _o = (Bright.Serialization.ByteBuf)translator.GetObject(L, 2, typeof(Bright.Serialization.ByteBuf));
                    
                    gen_to_be_invoked.WriteByteBufWithoutSize( _o );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TryReadByte(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    byte _x;
                    
                        var gen_ret = gen_to_be_invoked.TryReadByte( out _x );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    LuaAPI.xlua_pushinteger(L, _x);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TryDeserializeInplaceByteBuf(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _maxSize = LuaAPI.xlua_tointeger(L, 2);
                    Bright.Serialization.ByteBuf _inplaceTempBody = (Bright.Serialization.ByteBuf)translator.GetObject(L, 3, typeof(Bright.Serialization.ByteBuf));
                    
                        var gen_ret = gen_to_be_invoked.TryDeserializeInplaceByteBuf( _maxSize, _inplaceTempBody );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteRawTag(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    byte _b1 = (byte)LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.WriteRawTag( _b1 );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    byte _b1 = (byte)LuaAPI.xlua_tointeger(L, 2);
                    byte _b2 = (byte)LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.WriteRawTag( _b1, _b2 );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    byte _b1 = (byte)LuaAPI.xlua_tointeger(L, 2);
                    byte _b2 = (byte)LuaAPI.xlua_tointeger(L, 3);
                    byte _b3 = (byte)LuaAPI.xlua_tointeger(L, 4);
                    
                    gen_to_be_invoked.WriteRawTag( _b1, _b2, _b3 );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Bright.Serialization.ByteBuf.WriteRawTag!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BeginWriteSegment(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _oldSize;
                    
                    gen_to_be_invoked.BeginWriteSegment( out _oldSize );
                    LuaAPI.xlua_pushinteger(L, _oldSize);
                        
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_EndWriteSegment(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _oldSize = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.EndWriteSegment( _oldSize );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadSegment(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1) 
                {
                    int _startIndex;
                    int _segmentSize;
                    
                    gen_to_be_invoked.ReadSegment( out _startIndex, out _segmentSize );
                    LuaAPI.xlua_pushinteger(L, _startIndex);
                        
                    LuaAPI.xlua_pushinteger(L, _segmentSize);
                        
                    
                    
                    
                    return 2;
                }
                if(gen_param_count == 2&& translator.Assignable<Bright.Serialization.ByteBuf>(L, 2)) 
                {
                    Bright.Serialization.ByteBuf _buf = (Bright.Serialization.ByteBuf)translator.GetObject(L, 2, typeof(Bright.Serialization.ByteBuf));
                    
                    gen_to_be_invoked.ReadSegment( _buf );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Bright.Serialization.ByteBuf.ReadSegment!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_EnterSegment(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Bright.Serialization.SegmentSaveState _saveState;
                    
                    gen_to_be_invoked.EnterSegment( out _saveState );
                    translator.Push(L, _saveState);
                        
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LeaveSegment(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Bright.Serialization.SegmentSaveState _saveState;translator.Get(L, 2, out _saveState);
                    
                    gen_to_be_invoked.LeaveSegment( _saveState );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ToString(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ToString(  );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Equals(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<object>(L, 2)) 
                {
                    object _obj = translator.GetObject(L, 2, typeof(object));
                    
                        var gen_ret = gen_to_be_invoked.Equals( _obj );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<Bright.Serialization.ByteBuf>(L, 2)) 
                {
                    Bright.Serialization.ByteBuf _other = (Bright.Serialization.ByteBuf)translator.GetObject(L, 2, typeof(Bright.Serialization.ByteBuf));
                    
                        var gen_ret = gen_to_be_invoked.Equals( _other );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Bright.Serialization.ByteBuf.Equals!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Clone(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.Clone(  );
                        translator.PushAny(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FromString_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _value = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = Bright.Serialization.ByteBuf.FromString( _value );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetHashCode(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetHashCode(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Release(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Release(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteUnityVector2(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector2 _v;translator.Get(L, 2, out _v);
                    
                    gen_to_be_invoked.WriteUnityVector2( _v );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadUnityVector2(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ReadUnityVector2(  );
                        translator.PushUnityEngineVector2(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteUnityVector3(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _v;translator.Get(L, 2, out _v);
                    
                    gen_to_be_invoked.WriteUnityVector3( _v );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadUnityVector3(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ReadUnityVector3(  );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteUnityVector4(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector4 _v;translator.Get(L, 2, out _v);
                    
                    gen_to_be_invoked.WriteUnityVector4( _v );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadUnityVector4(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ReadUnityVector4(  );
                        translator.PushUnityEngineVector4(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ReaderIndex(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.ReaderIndex);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_WriterIndex(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.WriterIndex);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Capacity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.Capacity);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Size(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.Size);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Empty(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.Empty);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_NotEmpty(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.NotEmpty);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Bytes(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.Bytes);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Remaining(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.Remaining);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_NotCompactWritable(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.NotCompactWritable);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_StringCacheFinder(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Bright.Serialization.ByteBuf.StringCacheFinder);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ReaderIndex(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ReaderIndex = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_WriterIndex(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Bright.Serialization.ByteBuf gen_to_be_invoked = (Bright.Serialization.ByteBuf)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.WriterIndex = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_StringCacheFinder(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    Bright.Serialization.ByteBuf.StringCacheFinder = translator.GetDelegate<System.Func<byte[], int, int, string>>(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
