; ModuleID = 'marshal_methods.x86_64.ll'
source_filename = "marshal_methods.x86_64.ll"
target datalayout = "e-m:e-p270:32:32-p271:32:32-p272:64:64-i64:64-f80:128-n8:16:32:64-S128"
target triple = "x86_64-unknown-linux-android21"

%struct.MarshalMethodName = type {
	i64, ; uint64_t id
	ptr ; char* name
}

%struct.MarshalMethodsManagedClass = type {
	i32, ; uint32_t token
	ptr ; MonoClass klass
}

@assembly_image_cache = dso_local local_unnamed_addr global [172 x ptr] zeroinitializer, align 16

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [516 x i64] [
	i64 u0x0071cf2d27b7d61e, ; 0: lib_Xamarin.AndroidX.SwipeRefreshLayout.dll.so => 90
	i64 u0x02123411c4e01926, ; 1: lib_Xamarin.AndroidX.Navigation.Runtime.dll.so => 85
	i64 u0x022e81ea9c46e03a, ; 2: lib_CommunityToolkit.Maui.Core.dll.so => 36
	i64 u0x02abedc11addc1ed, ; 3: lib_Mono.Android.Runtime.dll.so => 170
	i64 u0x032267b2a94db371, ; 4: lib_Xamarin.AndroidX.AppCompat.dll.so => 67
	i64 u0x0363ac97a4cb84e6, ; 5: SQLitePCLRaw.provider.e_sqlite3.dll => 61
	i64 u0x043032f1d071fae0, ; 6: ru/Microsoft.Maui.Controls.resources => 24
	i64 u0x044440a55165631e, ; 7: lib-cs-Microsoft.Maui.Controls.resources.dll.so => 2
	i64 u0x046eb1581a80c6b0, ; 8: vi/Microsoft.Maui.Controls.resources => 30
	i64 u0x0517ef04e06e9f76, ; 9: System.Net.Primitives => 130
	i64 u0x0565d18c6da3de38, ; 10: Xamarin.AndroidX.RecyclerView => 87
	i64 u0x0581db89237110e9, ; 11: lib_System.Collections.dll.so => 104
	i64 u0x05989cb940b225a9, ; 12: Microsoft.Maui.dll => 53
	i64 u0x06076b5d2b581f08, ; 13: zh-HK/Microsoft.Maui.Controls.resources => 31
	i64 u0x06388ffe9f6c161a, ; 14: System.Xml.Linq.dll => 162
	i64 u0x0680a433c781bb3d, ; 15: Xamarin.AndroidX.Collection.Jvm => 71
	i64 u0x07c57877c7ba78ad, ; 16: ru/Microsoft.Maui.Controls.resources.dll => 24
	i64 u0x07dcdc7460a0c5e4, ; 17: System.Collections.NonGeneric => 102
	i64 u0x08122e52765333c8, ; 18: lib_Microsoft.Extensions.Logging.Debug.dll.so => 48
	i64 u0x08a7c865576bbde7, ; 19: System.Reflection.Primitives => 145
	i64 u0x08f3c9788ee2153c, ; 20: Xamarin.AndroidX.DrawerLayout => 76
	i64 u0x0919c28b89381a0b, ; 21: lib_Microsoft.Extensions.Options.dll.so => 49
	i64 u0x092266563089ae3e, ; 22: lib_System.Collections.NonGeneric.dll.so => 102
	i64 u0x09d144a7e214d457, ; 23: System.Security.Cryptography => 153
	i64 u0x09e2b9f743db21a8, ; 24: lib_System.Reflection.Metadata.dll.so => 144
	i64 u0x0abb3e2b271edc45, ; 25: System.Threading.Channels.dll => 158
	i64 u0x0b3b632c3bbee20c, ; 26: sk/Microsoft.Maui.Controls.resources => 25
	i64 u0x0b6aff547b84fbe9, ; 27: Xamarin.KotlinX.Serialization.Core.Jvm => 97
	i64 u0x0be2e1f8ce4064ed, ; 28: Xamarin.AndroidX.ViewPager => 91
	i64 u0x0c3ca6cc978e2aae, ; 29: pt-BR/Microsoft.Maui.Controls.resources => 21
	i64 u0x0c59ad9fbbd43abe, ; 30: Mono.Android => 171
	i64 u0x0c7790f60165fc06, ; 31: lib_Microsoft.Maui.Essentials.dll.so => 54
	i64 u0x0c83c82812e96127, ; 32: lib_System.Net.Mail.dll.so => 127
	i64 u0x0cce4bce83380b7f, ; 33: Xamarin.AndroidX.Security.SecurityCrypto => 89
	i64 u0x0e14e73a54dda68e, ; 34: lib_System.Net.NameResolution.dll.so => 128
	i64 u0x102a31b45304b1da, ; 35: Xamarin.AndroidX.CustomView => 75
	i64 u0x10f6cfcbcf801616, ; 36: System.IO.Compression.Brotli => 118
	i64 u0x11a603952763e1d4, ; 37: System.Net.Mail => 127
	i64 u0x123639456fb056da, ; 38: System.Reflection.Emit.Lightweight.dll => 143
	i64 u0x125b7f94acb989db, ; 39: Xamarin.AndroidX.RecyclerView.dll => 87
	i64 u0x138567fa954faa55, ; 40: Xamarin.AndroidX.Browser => 69
	i64 u0x13a01de0cbc3f06c, ; 41: lib-fr-Microsoft.Maui.Controls.resources.dll.so => 8
	i64 u0x13f1e5e209e91af4, ; 42: lib_Java.Interop.dll.so => 169
	i64 u0x13f1e880c25d96d1, ; 43: he/Microsoft.Maui.Controls.resources => 9
	i64 u0x143d8ea60a6a4011, ; 44: Microsoft.Extensions.DependencyInjection.Abstractions => 42
	i64 u0x159cc6c81072f00e, ; 45: lib_System.Diagnostics.EventLog.dll.so => 64
	i64 u0x17125c9a85b4929f, ; 46: lib_netstandard.dll.so => 167
	i64 u0x17b56e25558a5d36, ; 47: lib-hu-Microsoft.Maui.Controls.resources.dll.so => 12
	i64 u0x17f9358913beb16a, ; 48: System.Text.Encodings.Web => 155
	i64 u0x18402a709e357f3b, ; 49: lib_Xamarin.KotlinX.Serialization.Core.Jvm.dll.so => 97
	i64 u0x18a9befae51bb361, ; 50: System.Net.WebClient => 135
	i64 u0x18f0ce884e87d89a, ; 51: nb/Microsoft.Maui.Controls.resources.dll => 18
	i64 u0x1a91866a319e9259, ; 52: lib_System.Collections.Concurrent.dll.so => 100
	i64 u0x1aac34d1917ba5d3, ; 53: lib_System.dll.so => 166
	i64 u0x1aad60783ffa3e5b, ; 54: lib-th-Microsoft.Maui.Controls.resources.dll.so => 27
	i64 u0x1c292b1598348d77, ; 55: Microsoft.Extensions.Diagnostics.dll => 43
	i64 u0x1c753b5ff15bce1b, ; 56: Mono.Android.Runtime.dll => 170
	i64 u0x1da4110562816681, ; 57: Xamarin.AndroidX.Security.SecurityCrypto.dll => 89
	i64 u0x1e3d87657e9659bc, ; 58: Xamarin.AndroidX.Navigation.UI => 86
	i64 u0x1e71143913d56c10, ; 59: lib-ko-Microsoft.Maui.Controls.resources.dll.so => 16
	i64 u0x1ed8fcce5e9b50a0, ; 60: Microsoft.Extensions.Options.dll => 49
	i64 u0x1f055d15d807e1b2, ; 61: System.Xml.XmlSerializer => 165
	i64 u0x1f1ed22c1085f044, ; 62: lib_System.Diagnostics.FileVersionInfo.dll.so => 111
	i64 u0x20237ea48006d7a8, ; 63: lib_System.Net.WebClient.dll.so => 135
	i64 u0x209375905fcc1bad, ; 64: lib_System.IO.Compression.Brotli.dll.so => 118
	i64 u0x20fab3cf2dfbc8df, ; 65: lib_System.Diagnostics.Process.dll.so => 112
	i64 u0x2174319c0d835bc9, ; 66: System.Runtime => 152
	i64 u0x21cc7e445dcd5469, ; 67: System.Reflection.Emit.ILGeneration => 142
	i64 u0x220fd4f2e7c48170, ; 68: th/Microsoft.Maui.Controls.resources => 27
	i64 u0x237be844f1f812c7, ; 69: System.Threading.Thread.dll => 159
	i64 u0x2407aef2bbe8fadf, ; 70: System.Console => 108
	i64 u0x240abe014b27e7d3, ; 71: Xamarin.AndroidX.Core.dll => 73
	i64 u0x247619fe4413f8bf, ; 72: System.Runtime.Serialization.Primitives.dll => 151
	i64 u0x252073cc3caa62c2, ; 73: fr/Microsoft.Maui.Controls.resources.dll => 8
	i64 u0x256b8d41255f01b1, ; 74: Xamarin.Google.Crypto.Tink.Android => 94
	i64 u0x25a0a7eff76ea08e, ; 75: SQLitePCLRaw.batteries_v2.dll => 58
	i64 u0x2662c629b96b0b30, ; 76: lib_Xamarin.Kotlin.StdLib.dll.so => 95
	i64 u0x268c1439f13bcc29, ; 77: lib_Microsoft.Extensions.Primitives.dll.so => 50
	i64 u0x273f3515de5faf0d, ; 78: id/Microsoft.Maui.Controls.resources.dll => 13
	i64 u0x2742545f9094896d, ; 79: hr/Microsoft.Maui.Controls.resources => 11
	i64 u0x27b2b16f3e9de038, ; 80: Xamarin.Google.Crypto.Tink.Android.dll => 94
	i64 u0x27b410442fad6cf1, ; 81: Java.Interop.dll => 169
	i64 u0x2801845a2c71fbfb, ; 82: System.Net.Primitives.dll => 130
	i64 u0x28e52865585a1ebe, ; 83: Microsoft.Extensions.Diagnostics.Abstractions.dll => 44
	i64 u0x29cc7380d602a223, ; 84: Stripe.net => 62
	i64 u0x2a128783efe70ba0, ; 85: uk/Microsoft.Maui.Controls.resources.dll => 29
	i64 u0x2a3b095612184159, ; 86: lib_System.Net.NetworkInformation.dll.so => 129
	i64 u0x2a6507a5ffabdf28, ; 87: System.Diagnostics.TraceSource.dll => 114
	i64 u0x2ad156c8e1354139, ; 88: fi/Microsoft.Maui.Controls.resources => 7
	i64 u0x2af298f63581d886, ; 89: System.Text.RegularExpressions.dll => 157
	i64 u0x2afc1c4f898552ee, ; 90: lib_System.Formats.Asn1.dll.so => 117
	i64 u0x2b148910ed40fbf9, ; 91: zh-Hant/Microsoft.Maui.Controls.resources.dll => 33
	i64 u0x2c8bd14bb93a7d82, ; 92: lib-pl-Microsoft.Maui.Controls.resources.dll.so => 20
	i64 u0x2cc9e1fed6257257, ; 93: lib_System.Reflection.Emit.Lightweight.dll.so => 143
	i64 u0x2cd723e9fe623c7c, ; 94: lib_System.Private.Xml.Linq.dll.so => 140
	i64 u0x2d169d318a968379, ; 95: System.Threading.dll => 160
	i64 u0x2d47774b7d993f59, ; 96: sv/Microsoft.Maui.Controls.resources.dll => 26
	i64 u0x2db915caf23548d2, ; 97: System.Text.Json.dll => 156
	i64 u0x2e6f1f226821322a, ; 98: el/Microsoft.Maui.Controls.resources.dll => 5
	i64 u0x2f02f94df3200fe5, ; 99: System.Diagnostics.Process => 112
	i64 u0x2f2e98e1c89b1aff, ; 100: System.Xml.ReaderWriter => 163
	i64 u0x2ff49de6a71764a1, ; 101: lib_Microsoft.Extensions.Http.dll.so => 45
	i64 u0x309ee9eeec09a71e, ; 102: lib_Xamarin.AndroidX.Fragment.dll.so => 77
	i64 u0x31195fef5d8fb552, ; 103: _Microsoft.Android.Resource.Designer.dll => 34
	i64 u0x32243413e774362a, ; 104: Xamarin.AndroidX.CardView.dll => 70
	i64 u0x3235427f8d12dae1, ; 105: lib_System.Drawing.Primitives.dll.so => 115
	i64 u0x329753a17a517811, ; 106: fr/Microsoft.Maui.Controls.resources => 8
	i64 u0x32aa989ff07a84ff, ; 107: lib_System.Xml.ReaderWriter.dll.so => 163
	i64 u0x33829542f112d59b, ; 108: System.Collections.Immutable => 101
	i64 u0x33a31443733849fe, ; 109: lib-es-Microsoft.Maui.Controls.resources.dll.so => 6
	i64 u0x341abc357fbb4ebf, ; 110: lib_System.Net.Sockets.dll.so => 134
	i64 u0x34dfd74fe2afcf37, ; 111: Microsoft.Maui => 53
	i64 u0x34e292762d9615df, ; 112: cs/Microsoft.Maui.Controls.resources.dll => 2
	i64 u0x3508234247f48404, ; 113: Microsoft.Maui.Controls => 51
	i64 u0x3549870798b4cd30, ; 114: lib_Xamarin.AndroidX.ViewPager2.dll.so => 92
	i64 u0x355282fc1c909694, ; 115: Microsoft.Extensions.Configuration => 38
	i64 u0x36b2b50fdf589ae2, ; 116: System.Reflection.Emit.Lightweight => 143
	i64 u0x36cada77dc79928b, ; 117: System.IO.MemoryMappedFiles => 120
	i64 u0x374ef46b06791af6, ; 118: System.Reflection.Primitives.dll => 145
	i64 u0x380134e03b1e160a, ; 119: System.Collections.Immutable.dll => 101
	i64 u0x385c17636bb6fe6e, ; 120: Xamarin.AndroidX.CustomView.dll => 75
	i64 u0x38869c811d74050e, ; 121: System.Net.NameResolution.dll => 128
	i64 u0x39251dccb84bdcaa, ; 122: lib_System.Configuration.ConfigurationManager.dll.so => 63
	i64 u0x393c226616977fdb, ; 123: lib_Xamarin.AndroidX.ViewPager.dll.so => 91
	i64 u0x395e37c3334cf82a, ; 124: lib-ca-Microsoft.Maui.Controls.resources.dll.so => 1
	i64 u0x39aa39fda111d9d3, ; 125: Newtonsoft.Json => 56
	i64 u0x3b860f9932505633, ; 126: lib_System.Text.Encoding.Extensions.dll.so => 154
	i64 u0x3c7c495f58ac5ee9, ; 127: Xamarin.Kotlin.StdLib => 95
	i64 u0x3cd9d281d402eb9b, ; 128: Xamarin.AndroidX.Browser.dll => 69
	i64 u0x3d46f0b995082740, ; 129: System.Xml.Linq => 162
	i64 u0x3d9c2a242b040a50, ; 130: lib_Xamarin.AndroidX.Core.dll.so => 73
	i64 u0x3da7781d6333a8fe, ; 131: SQLitePCLRaw.batteries_v2 => 58
	i64 u0x407a10bb4bf95829, ; 132: lib_Xamarin.AndroidX.Navigation.Common.dll.so => 83
	i64 u0x415e36f6b13ff6f3, ; 133: System.Configuration.ConfigurationManager.dll => 63
	i64 u0x41cab042be111c34, ; 134: lib_Xamarin.AndroidX.AppCompat.AppCompatResources.dll.so => 68
	i64 u0x43375950ec7c1b6a, ; 135: netstandard.dll => 167
	i64 u0x434c4e1d9284cdae, ; 136: Mono.Android.dll => 171
	i64 u0x43950f84de7cc79a, ; 137: pl/Microsoft.Maui.Controls.resources.dll => 20
	i64 u0x448bd33429269b19, ; 138: Microsoft.CSharp => 99
	i64 u0x4499fa3c8e494654, ; 139: lib_System.Runtime.Serialization.Primitives.dll.so => 151
	i64 u0x4515080865a951a5, ; 140: Xamarin.Kotlin.StdLib.dll => 95
	i64 u0x45c40276a42e283e, ; 141: System.Diagnostics.TraceSource => 114
	i64 u0x46a4213bc97fe5ae, ; 142: lib-ru-Microsoft.Maui.Controls.resources.dll.so => 24
	i64 u0x47358bd471172e1d, ; 143: lib_System.Xml.Linq.dll.so => 162
	i64 u0x47daf4e1afbada10, ; 144: pt/Microsoft.Maui.Controls.resources => 22
	i64 u0x480c0a47dd42dd81, ; 145: lib_System.IO.MemoryMappedFiles.dll.so => 120
	i64 u0x49e952f19a4e2022, ; 146: System.ObjectModel => 138
	i64 u0x4a5667b2462a664b, ; 147: lib_Xamarin.AndroidX.Navigation.UI.dll.so => 86
	i64 u0x4b7b6532ded934b7, ; 148: System.Text.Json => 156
	i64 u0x4c7755cf07ad2d5f, ; 149: System.Net.Http.Json.dll => 125
	i64 u0x4cc5f15266470798, ; 150: lib_Xamarin.AndroidX.Loader.dll.so => 82
	i64 u0x4cf6f67dc77aacd2, ; 151: System.Net.NetworkInformation.dll => 129
	i64 u0x4d479f968a05e504, ; 152: System.Linq.Expressions.dll => 122
	i64 u0x4d55a010ffc4faff, ; 153: System.Private.Xml => 141
	i64 u0x4d95fccc1f67c7ca, ; 154: System.Runtime.Loader.dll => 148
	i64 u0x4dcf44c3c9b076a2, ; 155: it/Microsoft.Maui.Controls.resources.dll => 14
	i64 u0x4dd9247f1d2c3235, ; 156: Xamarin.AndroidX.Loader.dll => 82
	i64 u0x4e32f00cb0937401, ; 157: Mono.Android.Runtime => 170
	i64 u0x4ebd0c4b82c5eefc, ; 158: lib_System.Threading.Channels.dll.so => 158
	i64 u0x4f21ee6ef9eb527e, ; 159: ca/Microsoft.Maui.Controls.resources => 1
	i64 u0x4fd5f3ee53d0a4f0, ; 160: SQLitePCLRaw.lib.e_sqlite3.android => 60
	i64 u0x5037f0be3c28c7a3, ; 161: lib_Microsoft.Maui.Controls.dll.so => 51
	i64 u0x5112ed116d87baf8, ; 162: CommunityToolkit.Mvvm => 37
	i64 u0x5131bbe80989093f, ; 163: Xamarin.AndroidX.Lifecycle.ViewModel.Android.dll => 80
	i64 u0x51bb8a2afe774e32, ; 164: System.Drawing => 116
	i64 u0x526ce79eb8e90527, ; 165: lib_System.Net.Primitives.dll.so => 130
	i64 u0x52829f00b4467c38, ; 166: lib_System.Data.Common.dll.so => 109
	i64 u0x529ffe06f39ab8db, ; 167: Xamarin.AndroidX.Core => 73
	i64 u0x52ff996554dbf352, ; 168: Microsoft.Maui.Graphics => 55
	i64 u0x535f7e40e8fef8af, ; 169: lib-sk-Microsoft.Maui.Controls.resources.dll.so => 25
	i64 u0x53a96d5c86c9e194, ; 170: System.Net.NetworkInformation => 129
	i64 u0x53be1038a61e8d44, ; 171: System.Runtime.InteropServices.RuntimeInformation.dll => 146
	i64 u0x53c3014b9437e684, ; 172: lib-zh-HK-Microsoft.Maui.Controls.resources.dll.so => 31
	i64 u0x54795225dd1587af, ; 173: lib_System.Runtime.dll.so => 152
	i64 u0x556e8b63b660ab8b, ; 174: Xamarin.AndroidX.Lifecycle.Common.Jvm.dll => 78
	i64 u0x5588627c9a108ec9, ; 175: System.Collections.Specialized => 103
	i64 u0x55c62636751e5d4d, ; 176: MuizzaApp1.dll => 98
	i64 u0x571c5cfbec5ae8e2, ; 177: System.Private.Uri => 139
	i64 u0x578cd35c91d7b347, ; 178: lib_SQLitePCLRaw.core.dll.so => 59
	i64 u0x579a06fed6eec900, ; 179: System.Private.CoreLib.dll => 168
	i64 u0x57c542c14049b66d, ; 180: System.Diagnostics.DiagnosticSource => 110
	i64 u0x58601b2dda4a27b9, ; 181: lib-ja-Microsoft.Maui.Controls.resources.dll.so => 15
	i64 u0x58688d9af496b168, ; 182: Microsoft.Extensions.DependencyInjection.dll => 41
	i64 u0x595a356d23e8da9a, ; 183: lib_Microsoft.CSharp.dll.so => 99
	i64 u0x5a89a886ae30258d, ; 184: lib_Xamarin.AndroidX.CoordinatorLayout.dll.so => 72
	i64 u0x5a8f6699f4a1caa9, ; 185: lib_System.Threading.dll.so => 160
	i64 u0x5ae9cd33b15841bf, ; 186: System.ComponentModel => 107
	i64 u0x5b5f0e240a06a2a2, ; 187: da/Microsoft.Maui.Controls.resources.dll => 3
	i64 u0x5c393624b8176517, ; 188: lib_Microsoft.Extensions.Logging.dll.so => 46
	i64 u0x5c53c29f5073b0c9, ; 189: System.Diagnostics.FileVersionInfo => 111
	i64 u0x5d0a4a29b02d9d3c, ; 190: System.Net.WebHeaderCollection.dll => 136
	i64 u0x5db0cbbd1028510e, ; 191: lib_System.Runtime.InteropServices.dll.so => 147
	i64 u0x5db30905d3e5013b, ; 192: Xamarin.AndroidX.Collection.Jvm.dll => 71
	i64 u0x5e467bc8f09ad026, ; 193: System.Collections.Specialized.dll => 103
	i64 u0x5ea92fdb19ec8c4c, ; 194: System.Text.Encodings.Web.dll => 155
	i64 u0x5eb8046dd40e9ac3, ; 195: System.ComponentModel.Primitives => 105
	i64 u0x5f36ccf5c6a57e24, ; 196: System.Xml.ReaderWriter.dll => 163
	i64 u0x5f4294b9b63cb842, ; 197: System.Data.Common => 109
	i64 u0x5f7399e166075632, ; 198: lib_SQLitePCLRaw.lib.e_sqlite3.android.dll.so => 60
	i64 u0x5f9a2d823f664957, ; 199: lib-el-Microsoft.Maui.Controls.resources.dll.so => 5
	i64 u0x609f4b7b63d802d4, ; 200: lib_Microsoft.Extensions.DependencyInjection.dll.so => 41
	i64 u0x60cd4e33d7e60134, ; 201: Xamarin.KotlinX.Coroutines.Core.Jvm => 96
	i64 u0x60f62d786afcf130, ; 202: System.Memory => 124
	i64 u0x61be8d1299194243, ; 203: Microsoft.Maui.Controls.Xaml => 52
	i64 u0x61d2cba29557038f, ; 204: de/Microsoft.Maui.Controls.resources => 4
	i64 u0x61d88f399afb2f45, ; 205: lib_System.Runtime.Loader.dll.so => 148
	i64 u0x622eef6f9e59068d, ; 206: System.Private.CoreLib => 168
	i64 u0x63f1f6883c1e23c2, ; 207: lib_System.Collections.Immutable.dll.so => 101
	i64 u0x6400f68068c1e9f1, ; 208: Xamarin.Google.Android.Material.dll => 93
	i64 u0x658f524e4aba7dad, ; 209: CommunityToolkit.Maui.dll => 35
	i64 u0x65ecac39144dd3cc, ; 210: Microsoft.Maui.Controls.dll => 51
	i64 u0x65ece51227bfa724, ; 211: lib_System.Runtime.Numerics.dll.so => 149
	i64 u0x6692e924eade1b29, ; 212: lib_System.Console.dll.so => 108
	i64 u0x66a4e5c6a3fb0bae, ; 213: lib_Xamarin.AndroidX.Lifecycle.ViewModel.Android.dll.so => 80
	i64 u0x66d13304ce1a3efa, ; 214: Xamarin.AndroidX.CursorAdapter => 74
	i64 u0x68558ec653afa616, ; 215: lib-da-Microsoft.Maui.Controls.resources.dll.so => 3
	i64 u0x6872ec7a2e36b1ac, ; 216: System.Drawing.Primitives.dll => 115
	i64 u0x68fbbbe2eb455198, ; 217: System.Formats.Asn1 => 117
	i64 u0x69063fc0ba8e6bdd, ; 218: he/Microsoft.Maui.Controls.resources.dll => 9
	i64 u0x699dffb2427a2d71, ; 219: SQLitePCLRaw.lib.e_sqlite3.android.dll => 60
	i64 u0x6a4d7577b2317255, ; 220: System.Runtime.InteropServices.dll => 147
	i64 u0x6ace3b74b15ee4a4, ; 221: nb/Microsoft.Maui.Controls.resources => 18
	i64 u0x6d0a12b2adba20d8, ; 222: System.Security.Cryptography.ProtectedData.dll => 65
	i64 u0x6d12bfaa99c72b1f, ; 223: lib_Microsoft.Maui.Graphics.dll.so => 55
	i64 u0x6d79993361e10ef2, ; 224: Microsoft.Extensions.Primitives => 50
	i64 u0x6d86d56b84c8eb71, ; 225: lib_Xamarin.AndroidX.CursorAdapter.dll.so => 74
	i64 u0x6d9bea6b3e895cf7, ; 226: Microsoft.Extensions.Primitives.dll => 50
	i64 u0x6e25a02c3833319a, ; 227: lib_Xamarin.AndroidX.Navigation.Fragment.dll.so => 84
	i64 u0x6fd2265da78b93a4, ; 228: lib_Microsoft.Maui.dll.so => 53
	i64 u0x6fdfc7de82c33008, ; 229: cs/Microsoft.Maui.Controls.resources => 2
	i64 u0x70e99f48c05cb921, ; 230: tr/Microsoft.Maui.Controls.resources.dll => 28
	i64 u0x70fd3deda22442d2, ; 231: lib-nb-Microsoft.Maui.Controls.resources.dll.so => 18
	i64 u0x717530326f808838, ; 232: lib_Microsoft.Extensions.Diagnostics.Abstractions.dll.so => 44
	i64 u0x71a495ea3761dde8, ; 233: lib-it-Microsoft.Maui.Controls.resources.dll.so => 14
	i64 u0x71ad672adbe48f35, ; 234: System.ComponentModel.Primitives.dll => 105
	i64 u0x72b1fb4109e08d7b, ; 235: lib-hr-Microsoft.Maui.Controls.resources.dll.so => 11
	i64 u0x73e4ce94e2eb6ffc, ; 236: lib_System.Memory.dll.so => 124
	i64 u0x746cf89b511b4d40, ; 237: lib_Microsoft.Extensions.Diagnostics.dll.so => 43
	i64 u0x755a91767330b3d4, ; 238: lib_Microsoft.Extensions.Configuration.dll.so => 38
	i64 u0x76012e7334db86e5, ; 239: lib_Xamarin.AndroidX.SavedState.dll.so => 88
	i64 u0x76ca07b878f44da0, ; 240: System.Runtime.Numerics.dll => 149
	i64 u0x779f67ad3b8efbd5, ; 241: Microsoft.Extensions.Configuration.Json.dll => 40
	i64 u0x780bc73597a503a9, ; 242: lib-ms-Microsoft.Maui.Controls.resources.dll.so => 17
	i64 u0x783606d1e53e7a1a, ; 243: th/Microsoft.Maui.Controls.resources.dll => 27
	i64 u0x78a45e51311409b6, ; 244: Xamarin.AndroidX.Fragment.dll => 77
	i64 u0x7a25bdb29108c6e7, ; 245: Microsoft.Extensions.Http => 45
	i64 u0x7adb8da2ac89b647, ; 246: fi/Microsoft.Maui.Controls.resources.dll => 7
	i64 u0x7bef86a4335c4870, ; 247: System.ComponentModel.TypeConverter => 106
	i64 u0x7c0820144cd34d6a, ; 248: sk/Microsoft.Maui.Controls.resources.dll => 25
	i64 u0x7c2a0bd1e0f988fc, ; 249: lib-de-Microsoft.Maui.Controls.resources.dll.so => 4
	i64 u0x7c41d387501568ba, ; 250: System.Net.WebClient.dll => 135
	i64 u0x7cc637f941f716d0, ; 251: CommunityToolkit.Maui.Core => 36
	i64 u0x7d649b75d580bb42, ; 252: ms/Microsoft.Maui.Controls.resources.dll => 17
	i64 u0x7d8ee2bdc8e3aad1, ; 253: System.Numerics.Vectors => 137
	i64 u0x7df5df8db8eaa6ac, ; 254: Microsoft.Extensions.Logging.Debug => 48
	i64 u0x7dfc3d6d9d8d7b70, ; 255: System.Collections => 104
	i64 u0x7e946809d6008ef2, ; 256: lib_System.ObjectModel.dll.so => 138
	i64 u0x7ecc13347c8fd849, ; 257: lib_System.ComponentModel.dll.so => 107
	i64 u0x7f00ddd9b9ca5a13, ; 258: Xamarin.AndroidX.ViewPager.dll => 91
	i64 u0x7f9351cd44b1273f, ; 259: Microsoft.Extensions.Configuration.Abstractions => 39
	i64 u0x7fbd557c99b3ce6f, ; 260: lib_Xamarin.AndroidX.Lifecycle.LiveData.Core.dll.so => 79
	i64 u0x80da183a87731838, ; 261: System.Reflection.Metadata => 144
	i64 u0x80fa55b6d1b0be99, ; 262: SQLitePCLRaw.provider.e_sqlite3 => 61
	i64 u0x812c069d5cdecc17, ; 263: System.dll => 166
	i64 u0x81ab745f6c0f5ce6, ; 264: zh-Hant/Microsoft.Maui.Controls.resources => 33
	i64 u0x8277f2be6b5ce05f, ; 265: Xamarin.AndroidX.AppCompat => 67
	i64 u0x828f06563b30bc50, ; 266: lib_Xamarin.AndroidX.CardView.dll.so => 70
	i64 u0x82df8f5532a10c59, ; 267: lib_System.Drawing.dll.so => 116
	i64 u0x82f6403342e12049, ; 268: uk/Microsoft.Maui.Controls.resources => 29
	i64 u0x83144699b312ad81, ; 269: SQLite-net.dll => 57
	i64 u0x83c14ba66c8e2b8c, ; 270: zh-Hans/Microsoft.Maui.Controls.resources => 32
	i64 u0x86a909228dc7657b, ; 271: lib-zh-Hant-Microsoft.Maui.Controls.resources.dll.so => 33
	i64 u0x86b3e00c36b84509, ; 272: Microsoft.Extensions.Configuration.dll => 38
	i64 u0x87c69b87d9283884, ; 273: lib_System.Threading.Thread.dll.so => 159
	i64 u0x87f6569b25707834, ; 274: System.IO.Compression.Brotli.dll => 118
	i64 u0x8842b3a5d2d3fb36, ; 275: Microsoft.Maui.Essentials => 54
	i64 u0x88bda98e0cffb7a9, ; 276: lib_Xamarin.KotlinX.Coroutines.Core.Jvm.dll.so => 96
	i64 u0x8930322c7bd8f768, ; 277: netstandard => 167
	i64 u0x897a606c9e39c75f, ; 278: lib_System.ComponentModel.Primitives.dll.so => 105
	i64 u0x89c5188089ec2cd5, ; 279: lib_System.Runtime.InteropServices.RuntimeInformation.dll.so => 146
	i64 u0x8ad229ea26432ee2, ; 280: Xamarin.AndroidX.Loader => 82
	i64 u0x8b4ff5d0fdd5faa1, ; 281: lib_System.Diagnostics.DiagnosticSource.dll.so => 110
	i64 u0x8b8d01333a96d0b5, ; 282: System.Diagnostics.Process.dll => 112
	i64 u0x8b9ceca7acae3451, ; 283: lib-he-Microsoft.Maui.Controls.resources.dll.so => 9
	i64 u0x8d0f420977c2c1c7, ; 284: Xamarin.AndroidX.CursorAdapter.dll => 74
	i64 u0x8d7b8ab4b3310ead, ; 285: System.Threading => 160
	i64 u0x8da188285aadfe8e, ; 286: System.Collections.Concurrent => 100
	i64 u0x8ec6e06a61c1baeb, ; 287: lib_Newtonsoft.Json.dll.so => 56
	i64 u0x8ed3cdd722b4d782, ; 288: System.Diagnostics.EventLog => 64
	i64 u0x8ed807bfe9858dfc, ; 289: Xamarin.AndroidX.Navigation.Common => 83
	i64 u0x8ee08b8194a30f48, ; 290: lib-hi-Microsoft.Maui.Controls.resources.dll.so => 10
	i64 u0x8ef7601039857a44, ; 291: lib-ro-Microsoft.Maui.Controls.resources.dll.so => 23
	i64 u0x8ef9414937d93a0a, ; 292: SQLitePCLRaw.core.dll => 59
	i64 u0x8f32c6f611f6ffab, ; 293: pt/Microsoft.Maui.Controls.resources.dll => 22
	i64 u0x8f8829d21c8985a4, ; 294: lib-pt-BR-Microsoft.Maui.Controls.resources.dll.so => 21
	i64 u0x8fd27d934d7b3a55, ; 295: SQLitePCLRaw.core => 59
	i64 u0x90263f8448b8f572, ; 296: lib_System.Diagnostics.TraceSource.dll.so => 114
	i64 u0x903101b46fb73a04, ; 297: _Microsoft.Android.Resource.Designer => 34
	i64 u0x90393bd4865292f3, ; 298: lib_System.IO.Compression.dll.so => 119
	i64 u0x90634f86c5ebe2b5, ; 299: Xamarin.AndroidX.Lifecycle.ViewModel.Android => 80
	i64 u0x907b636704ad79ef, ; 300: lib_Microsoft.Maui.Controls.Xaml.dll.so => 52
	i64 u0x91418dc638b29e68, ; 301: lib_Xamarin.AndroidX.CustomView.dll.so => 75
	i64 u0x914647982e998267, ; 302: Microsoft.Extensions.Configuration.Json => 40
	i64 u0x9157bd523cd7ed36, ; 303: lib_System.Text.Json.dll.so => 156
	i64 u0x91a74f07b30d37e2, ; 304: System.Linq.dll => 123
	i64 u0x91fa41a87223399f, ; 305: ca/Microsoft.Maui.Controls.resources.dll => 1
	i64 u0x93cfa73ab28d6e35, ; 306: ms/Microsoft.Maui.Controls.resources => 17
	i64 u0x944077d8ca3c6580, ; 307: System.IO.Compression.dll => 119
	i64 u0x967fc325e09bfa8c, ; 308: es/Microsoft.Maui.Controls.resources => 6
	i64 u0x9732d8dbddea3d9a, ; 309: id/Microsoft.Maui.Controls.resources => 13
	i64 u0x978be80e5210d31b, ; 310: Microsoft.Maui.Graphics.dll => 55
	i64 u0x97b8c771ea3e4220, ; 311: System.ComponentModel.dll => 107
	i64 u0x97e144c9d3c6976e, ; 312: System.Collections.Concurrent.dll => 100
	i64 u0x991d510397f92d9d, ; 313: System.Linq.Expressions => 122
	i64 u0x999cb19e1a04ffd3, ; 314: CommunityToolkit.Mvvm.dll => 37
	i64 u0x99a00ca5270c6878, ; 315: Xamarin.AndroidX.Navigation.Runtime => 85
	i64 u0x99cdc6d1f2d3a72f, ; 316: ko/Microsoft.Maui.Controls.resources.dll => 16
	i64 u0x9d5dbcf5a48583fe, ; 317: lib_Xamarin.AndroidX.Activity.dll.so => 66
	i64 u0x9d74dee1a7725f34, ; 318: Microsoft.Extensions.Configuration.Abstractions.dll => 39
	i64 u0x9e4534b6adaf6e84, ; 319: nl/Microsoft.Maui.Controls.resources => 19
	i64 u0x9eaf1efdf6f7267e, ; 320: Xamarin.AndroidX.Navigation.Common.dll => 83
	i64 u0x9ef542cf1f78c506, ; 321: Xamarin.AndroidX.Lifecycle.LiveData.Core => 79
	i64 u0xa0d8259f4cc284ec, ; 322: lib_System.Security.Cryptography.dll.so => 153
	i64 u0xa0e17ca50c77a225, ; 323: lib_Xamarin.Google.Crypto.Tink.Android.dll.so => 94
	i64 u0xa1440773ee9d341e, ; 324: Xamarin.Google.Android.Material => 93
	i64 u0xa1b9d7c27f47219f, ; 325: Xamarin.AndroidX.Navigation.UI.dll => 86
	i64 u0xa2572680829d2c7c, ; 326: System.IO.Pipelines.dll => 121
	i64 u0xa46aa1eaa214539b, ; 327: ko/Microsoft.Maui.Controls.resources => 16
	i64 u0xa4a372eecb9e4df0, ; 328: Microsoft.Extensions.Diagnostics => 43
	i64 u0xa4d20d2ff0563d26, ; 329: lib_CommunityToolkit.Mvvm.dll.so => 37
	i64 u0xa4edc8f2ceae241a, ; 330: System.Data.Common.dll => 109
	i64 u0xa5494f40f128ce6a, ; 331: System.Runtime.Serialization.Formatters.dll => 150
	i64 u0xa5e599d1e0524750, ; 332: System.Numerics.Vectors.dll => 137
	i64 u0xa5f1ba49b85dd355, ; 333: System.Security.Cryptography.dll => 153
	i64 u0xa61975a5a37873ea, ; 334: lib_System.Xml.XmlSerializer.dll.so => 165
	i64 u0xa67dbee13e1df9ca, ; 335: Xamarin.AndroidX.SavedState.dll => 88
	i64 u0xa684b098dd27b296, ; 336: lib_Xamarin.AndroidX.Security.SecurityCrypto.dll.so => 89
	i64 u0xa68a420042bb9b1f, ; 337: Xamarin.AndroidX.DrawerLayout.dll => 76
	i64 u0xa78ce3745383236a, ; 338: Xamarin.AndroidX.Lifecycle.Common.Jvm => 78
	i64 u0xa7c31b56b4dc7b33, ; 339: hu/Microsoft.Maui.Controls.resources => 12
	i64 u0xa964304b5631e28a, ; 340: CommunityToolkit.Maui.Core.dll => 36
	i64 u0xaa2219c8e3449ff5, ; 341: Microsoft.Extensions.Logging.Abstractions => 47
	i64 u0xaa443ac34067eeef, ; 342: System.Private.Xml.dll => 141
	i64 u0xaa52de307ef5d1dd, ; 343: System.Net.Http => 126
	i64 u0xaaaf86367285a918, ; 344: Microsoft.Extensions.DependencyInjection.Abstractions.dll => 42
	i64 u0xaaf84bb3f052a265, ; 345: el/Microsoft.Maui.Controls.resources => 5
	i64 u0xab9c1b2687d86b0b, ; 346: lib_System.Linq.Expressions.dll.so => 122
	i64 u0xac2af3fa195a15ce, ; 347: System.Runtime.Numerics => 149
	i64 u0xac5376a2a538dc10, ; 348: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 79
	i64 u0xac98d31068e24591, ; 349: System.Xml.XDocument => 164
	i64 u0xacd46e002c3ccb97, ; 350: ro/Microsoft.Maui.Controls.resources => 23
	i64 u0xacf42eea7ef9cd12, ; 351: System.Threading.Channels => 158
	i64 u0xad89c07347f1bad6, ; 352: nl/Microsoft.Maui.Controls.resources.dll => 19
	i64 u0xadbb53caf78a79d2, ; 353: System.Web.HttpUtility => 161
	i64 u0xadc90ab061a9e6e4, ; 354: System.ComponentModel.TypeConverter.dll => 106
	i64 u0xadf4cf30debbeb9a, ; 355: System.Net.ServicePoint.dll => 133
	i64 u0xadf511667bef3595, ; 356: System.Net.Security => 132
	i64 u0xae282bcd03739de7, ; 357: Java.Interop => 169
	i64 u0xae53579c90db1107, ; 358: System.ObjectModel.dll => 138
	i64 u0xae7ea18c61eef394, ; 359: SQLite-net => 57
	i64 u0xafe29f45095518e7, ; 360: lib_Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll.so => 81
	i64 u0xb05cc42cd94c6d9d, ; 361: lib-sv-Microsoft.Maui.Controls.resources.dll.so => 26
	i64 u0xb220631954820169, ; 362: System.Text.RegularExpressions => 157
	i64 u0xb2a3f67f3bf29fce, ; 363: da/Microsoft.Maui.Controls.resources => 3
	i64 u0xb398860d6ed7ba2f, ; 364: System.Security.Cryptography.ProtectedData => 65
	i64 u0xb3f0a0fcda8d3ebc, ; 365: Xamarin.AndroidX.CardView => 70
	i64 u0xb46be1aa6d4fff93, ; 366: hi/Microsoft.Maui.Controls.resources => 10
	i64 u0xb477491be13109d8, ; 367: ar/Microsoft.Maui.Controls.resources => 0
	i64 u0xb4bd7015ecee9d86, ; 368: System.IO.Pipelines => 121
	i64 u0xb4ff710863453fda, ; 369: System.Diagnostics.FileVersionInfo.dll => 111
	i64 u0xb5c7fcdafbc67ee4, ; 370: Microsoft.Extensions.Logging.Abstractions.dll => 47
	i64 u0xb7212c4683a94afe, ; 371: System.Drawing.Primitives => 115
	i64 u0xb7b7753d1f319409, ; 372: sv/Microsoft.Maui.Controls.resources => 26
	i64 u0xb81a2c6e0aee50fe, ; 373: lib_System.Private.CoreLib.dll.so => 168
	i64 u0xb872c26142d22aa9, ; 374: Microsoft.Extensions.Http.dll => 45
	i64 u0xb8c60af47c08d4da, ; 375: System.Net.ServicePoint => 133
	i64 u0xb9185c33a1643eed, ; 376: Microsoft.CSharp.dll => 99
	i64 u0xb9f64d3b230def68, ; 377: lib-pt-Microsoft.Maui.Controls.resources.dll.so => 22
	i64 u0xb9fc3c8a556e3691, ; 378: ja/Microsoft.Maui.Controls.resources => 15
	i64 u0xba4670aa94a2b3c6, ; 379: lib_System.Xml.XDocument.dll.so => 164
	i64 u0xba48785529705af9, ; 380: System.Collections.dll => 104
	i64 u0xbb65706fde942ce3, ; 381: System.Net.Sockets => 134
	i64 u0xbbd180354b67271a, ; 382: System.Runtime.Serialization.Formatters => 150
	i64 u0xbc22a245dab70cb4, ; 383: lib_SQLitePCLRaw.provider.e_sqlite3.dll.so => 61
	i64 u0xbd0e2c0d55246576, ; 384: System.Net.Http.dll => 126
	i64 u0xbd437a2cdb333d0d, ; 385: Xamarin.AndroidX.ViewPager2 => 92
	i64 u0xbd5d0b88d3d647a5, ; 386: lib_Xamarin.AndroidX.Browser.dll.so => 69
	i64 u0xbee38d4a88835966, ; 387: Xamarin.AndroidX.AppCompat.AppCompatResources => 68
	i64 u0xbfc1e1fb3095f2b3, ; 388: lib_System.Net.Http.Json.dll.so => 125
	i64 u0xc040a4ab55817f58, ; 389: ar/Microsoft.Maui.Controls.resources.dll => 0
	i64 u0xc0d928351ab5ca77, ; 390: System.Console.dll => 108
	i64 u0xc12b8b3afa48329c, ; 391: lib_System.Linq.dll.so => 123
	i64 u0xc1ff9ae3cdb6e1e6, ; 392: Xamarin.AndroidX.Activity.dll => 66
	i64 u0xc28c50f32f81cc73, ; 393: ja/Microsoft.Maui.Controls.resources.dll => 15
	i64 u0xc2bcfec99f69365e, ; 394: Xamarin.AndroidX.ViewPager2.dll => 92
	i64 u0xc4d3858ed4d08512, ; 395: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 81
	i64 u0xc50fded0ded1418c, ; 396: lib_System.ComponentModel.TypeConverter.dll.so => 106
	i64 u0xc519125d6bc8fb11, ; 397: lib_System.Net.Requests.dll.so => 131
	i64 u0xc5293b19e4dc230e, ; 398: Xamarin.AndroidX.Navigation.Fragment => 84
	i64 u0xc5325b2fcb37446f, ; 399: lib_System.Private.Xml.dll.so => 141
	i64 u0xc5a0f4b95a699af7, ; 400: lib_System.Private.Uri.dll.so => 139
	i64 u0xc7c01e7d7c93a110, ; 401: System.Text.Encoding.Extensions.dll => 154
	i64 u0xc7ce851898a4548e, ; 402: lib_System.Web.HttpUtility.dll.so => 161
	i64 u0xc858a28d9ee5a6c5, ; 403: lib_System.Collections.Specialized.dll.so => 103
	i64 u0xc9c62c8f354ac568, ; 404: lib_System.Diagnostics.TextWriterTraceListener.dll.so => 113
	i64 u0xc9e54b32fc19baf3, ; 405: lib_CommunityToolkit.Maui.dll.so => 35
	i64 u0xca3a723e7342c5b6, ; 406: lib-tr-Microsoft.Maui.Controls.resources.dll.so => 28
	i64 u0xcab3493c70141c2d, ; 407: pl/Microsoft.Maui.Controls.resources => 20
	i64 u0xcace8b9ca412599f, ; 408: MuizzaApp1 => 98
	i64 u0xcacfddc9f7c6de76, ; 409: ro/Microsoft.Maui.Controls.resources.dll => 23
	i64 u0xcbd4fdd9cef4a294, ; 410: lib__Microsoft.Android.Resource.Designer.dll.so => 34
	i64 u0xcc2876b32ef2794c, ; 411: lib_System.Text.RegularExpressions.dll.so => 157
	i64 u0xcc5c3bb714c4561e, ; 412: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 96
	i64 u0xcc76886e09b88260, ; 413: Xamarin.KotlinX.Serialization.Core.Jvm.dll => 97
	i64 u0xccf25c4b634ccd3a, ; 414: zh-Hans/Microsoft.Maui.Controls.resources.dll => 32
	i64 u0xccf78fa9cfab41e9, ; 415: lib_MuizzaApp1.dll.so => 98
	i64 u0xcd10a42808629144, ; 416: System.Net.Requests => 131
	i64 u0xcdd0c48b6937b21c, ; 417: Xamarin.AndroidX.SwipeRefreshLayout => 90
	i64 u0xcf23d8093f3ceadf, ; 418: System.Diagnostics.DiagnosticSource.dll => 110
	i64 u0xcf5ff6b6b2c4c382, ; 419: System.Net.Mail.dll => 127
	i64 u0xcf8fc898f98b0d34, ; 420: System.Private.Xml.Linq => 140
	i64 u0xd04b5f59ed596e31, ; 421: System.Reflection.Metadata.dll => 144
	i64 u0xd0de8a113e976700, ; 422: System.Diagnostics.TextWriterTraceListener => 113
	i64 u0xd1194e1d8a8de83c, ; 423: lib_Xamarin.AndroidX.Lifecycle.Common.Jvm.dll.so => 78
	i64 u0xd16fd7fb9bbcd43e, ; 424: Microsoft.Extensions.Diagnostics.Abstractions => 44
	i64 u0xd22a0c4630f2fe66, ; 425: lib_System.Security.Cryptography.ProtectedData.dll.so => 65
	i64 u0xd333d0af9e423810, ; 426: System.Runtime.InteropServices => 147
	i64 u0xd3426d966bb704f5, ; 427: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 68
	i64 u0xd3651b6fc3125825, ; 428: System.Private.Uri.dll => 139
	i64 u0xd373685349b1fe8b, ; 429: Microsoft.Extensions.Logging.dll => 46
	i64 u0xd3e4c8d6a2d5d470, ; 430: it/Microsoft.Maui.Controls.resources => 14
	i64 u0xd4645626dffec99d, ; 431: lib_Microsoft.Extensions.DependencyInjection.Abstractions.dll.so => 42
	i64 u0xd5507e11a2b2839f, ; 432: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 81
	i64 u0xd60815f26a12e140, ; 433: Microsoft.Extensions.Logging.Debug.dll => 48
	i64 u0xd6694f8359737e4e, ; 434: Xamarin.AndroidX.SavedState => 88
	i64 u0xd6d21782156bc35b, ; 435: Xamarin.AndroidX.SwipeRefreshLayout.dll => 90
	i64 u0xd72329819cbbbc44, ; 436: lib_Microsoft.Extensions.Configuration.Abstractions.dll.so => 39
	i64 u0xd72c760af136e863, ; 437: System.Xml.XmlSerializer.dll => 165
	i64 u0xd7b3764ada9d341d, ; 438: lib_Microsoft.Extensions.Logging.Abstractions.dll.so => 47
	i64 u0xda1dfa4c534a9251, ; 439: Microsoft.Extensions.DependencyInjection => 41
	i64 u0xdad05a11827959a3, ; 440: System.Collections.NonGeneric.dll => 102
	i64 u0xdb5383ab5865c007, ; 441: lib-vi-Microsoft.Maui.Controls.resources.dll.so => 30
	i64 u0xdb58816721c02a59, ; 442: lib_System.Reflection.Emit.ILGeneration.dll.so => 142
	i64 u0xdbeda89f832aa805, ; 443: vi/Microsoft.Maui.Controls.resources.dll => 30
	i64 u0xdbf9607a441b4505, ; 444: System.Linq => 123
	i64 u0xdce2c53525640bf3, ; 445: Microsoft.Extensions.Logging => 46
	i64 u0xdd2b722d78ef5f43, ; 446: System.Runtime.dll => 152
	i64 u0xdd67031857c72f96, ; 447: lib_System.Text.Encodings.Web.dll.so => 155
	i64 u0xdde30e6b77aa6f6c, ; 448: lib-zh-Hans-Microsoft.Maui.Controls.resources.dll.so => 32
	i64 u0xde110ae80fa7c2e2, ; 449: System.Xml.XDocument.dll => 164
	i64 u0xde8769ebda7d8647, ; 450: hr/Microsoft.Maui.Controls.resources.dll => 11
	i64 u0xe0142572c095a480, ; 451: Xamarin.AndroidX.AppCompat.dll => 67
	i64 u0xe02f89350ec78051, ; 452: Xamarin.AndroidX.CoordinatorLayout.dll => 72
	i64 u0xe192a588d4410686, ; 453: lib_System.IO.Pipelines.dll.so => 121
	i64 u0xe1a08bd3fa539e0d, ; 454: System.Runtime.Loader => 148
	i64 u0xe1b52f9f816c70ef, ; 455: System.Private.Xml.Linq.dll => 140
	i64 u0xe1ecfdb7fff86067, ; 456: System.Net.Security.dll => 132
	i64 u0xe22fa4c9c645db62, ; 457: System.Diagnostics.TextWriterTraceListener.dll => 113
	i64 u0xe2420585aeceb728, ; 458: System.Net.Requests.dll => 131
	i64 u0xe29b73bc11392966, ; 459: lib-id-Microsoft.Maui.Controls.resources.dll.so => 13
	i64 u0xe3811d68d4fe8463, ; 460: pt-BR/Microsoft.Maui.Controls.resources.dll => 21
	i64 u0xe3a586956771a0ed, ; 461: lib_SQLite-net.dll.so => 57
	i64 u0xe494f7ced4ecd10a, ; 462: hu/Microsoft.Maui.Controls.resources.dll => 12
	i64 u0xe4a9b1e40d1e8917, ; 463: lib-fi-Microsoft.Maui.Controls.resources.dll.so => 7
	i64 u0xe4f74a0b5bf9703f, ; 464: System.Runtime.Serialization.Primitives => 151
	i64 u0xe5434e8a119ceb69, ; 465: lib_Mono.Android.dll.so => 171
	i64 u0xe57d22ca4aeb4900, ; 466: System.Configuration.ConfigurationManager => 63
	i64 u0xe89a2a9ef110899b, ; 467: System.Drawing.dll => 116
	i64 u0xed6ef763c6fb395f, ; 468: System.Diagnostics.EventLog.dll => 64
	i64 u0xedc4817167106c23, ; 469: System.Net.Sockets.dll => 134
	i64 u0xedc632067fb20ff3, ; 470: System.Memory.dll => 124
	i64 u0xedc8e4ca71a02a8b, ; 471: Xamarin.AndroidX.Navigation.Runtime.dll => 85
	i64 u0xeeb7ebb80150501b, ; 472: lib_Xamarin.AndroidX.Collection.Jvm.dll.so => 71
	i64 u0xef72742e1bcca27a, ; 473: Microsoft.Maui.Essentials.dll => 54
	i64 u0xefec0b7fdc57ec42, ; 474: Xamarin.AndroidX.Activity => 66
	i64 u0xf00c29406ea45e19, ; 475: es/Microsoft.Maui.Controls.resources.dll => 6
	i64 u0xf09e47b6ae914f6e, ; 476: System.Net.NameResolution => 128
	i64 u0xf0bb49dadd3a1fe1, ; 477: lib_System.Net.ServicePoint.dll.so => 133
	i64 u0xf0de2537ee19c6ca, ; 478: lib_System.Net.WebHeaderCollection.dll.so => 136
	i64 u0xf11b621fc87b983f, ; 479: Microsoft.Maui.Controls.Xaml.dll => 52
	i64 u0xf1c4b4005493d871, ; 480: System.Formats.Asn1.dll => 117
	i64 u0xf238bd79489d3a96, ; 481: lib-nl-Microsoft.Maui.Controls.resources.dll.so => 19
	i64 u0xf37221fda4ef8830, ; 482: lib_Xamarin.Google.Android.Material.dll.so => 93
	i64 u0xf3ddfe05336abf29, ; 483: System => 166
	i64 u0xf408654b2a135055, ; 484: System.Reflection.Emit.ILGeneration.dll => 142
	i64 u0xf4c1dd70a5496a17, ; 485: System.IO.Compression => 119
	i64 u0xf5fc7602fe27b333, ; 486: System.Net.WebHeaderCollection => 136
	i64 u0xf6077741019d7428, ; 487: Xamarin.AndroidX.CoordinatorLayout => 72
	i64 u0xf6de7fa3776f8927, ; 488: lib_Microsoft.Extensions.Configuration.Json.dll.so => 40
	i64 u0xf77b20923f07c667, ; 489: de/Microsoft.Maui.Controls.resources.dll => 4
	i64 u0xf7e2cac4c45067b3, ; 490: lib_System.Numerics.Vectors.dll.so => 137
	i64 u0xf7e74930e0e3d214, ; 491: zh-HK/Microsoft.Maui.Controls.resources.dll => 31
	i64 u0xf7fa0bf77fe677cc, ; 492: Newtonsoft.Json.dll => 56
	i64 u0xf84773b5c81e3cef, ; 493: lib-uk-Microsoft.Maui.Controls.resources.dll.so => 29
	i64 u0xf8b77539b362d3ba, ; 494: lib_System.Reflection.Primitives.dll.so => 145
	i64 u0xf8e045dc345b2ea3, ; 495: lib_Xamarin.AndroidX.RecyclerView.dll.so => 87
	i64 u0xf915dc29808193a1, ; 496: System.Web.HttpUtility.dll => 161
	i64 u0xf96c777a2a0686f4, ; 497: hi/Microsoft.Maui.Controls.resources.dll => 10
	i64 u0xf9eec5bb3a6aedc6, ; 498: Microsoft.Extensions.Options => 49
	i64 u0xfa1821769c641466, ; 499: lib_Stripe.net.dll.so => 62
	i64 u0xfa3f278f288b0e84, ; 500: lib_System.Net.Security.dll.so => 132
	i64 u0xfa5ed7226d978949, ; 501: lib-ar-Microsoft.Maui.Controls.resources.dll.so => 0
	i64 u0xfa645d91e9fc4cba, ; 502: System.Threading.Thread => 159
	i64 u0xfb022853d73b7fa5, ; 503: lib_SQLitePCLRaw.batteries_v2.dll.so => 58
	i64 u0xfbf0a31c9fc34bc4, ; 504: lib_System.Net.Http.dll.so => 126
	i64 u0xfc6b7527cc280b3f, ; 505: lib_System.Runtime.Serialization.Formatters.dll.so => 150
	i64 u0xfc719aec26adf9d9, ; 506: Xamarin.AndroidX.Navigation.Fragment.dll => 84
	i64 u0xfcd302092ada6328, ; 507: System.IO.MemoryMappedFiles.dll => 120
	i64 u0xfd22f00870e40ae0, ; 508: lib_Xamarin.AndroidX.DrawerLayout.dll.so => 76
	i64 u0xfd49b3c1a76e2748, ; 509: System.Runtime.InteropServices.RuntimeInformation => 146
	i64 u0xfd536c702f64dc47, ; 510: System.Text.Encoding.Extensions => 154
	i64 u0xfd583f7657b6a1cb, ; 511: Xamarin.AndroidX.Fragment => 77
	i64 u0xfdbe4710aa9beeff, ; 512: CommunityToolkit.Maui => 35
	i64 u0xfeae9952cf03b8cb, ; 513: tr/Microsoft.Maui.Controls.resources => 28
	i64 u0xff3ae8ee96dbc2fb, ; 514: Stripe.net.dll => 62
	i64 u0xff9b54613e0d2cc8 ; 515: System.Net.Http.Json => 125
], align 16

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [516 x i32] [
	i32 90, i32 85, i32 36, i32 170, i32 67, i32 61, i32 24, i32 2,
	i32 30, i32 130, i32 87, i32 104, i32 53, i32 31, i32 162, i32 71,
	i32 24, i32 102, i32 48, i32 145, i32 76, i32 49, i32 102, i32 153,
	i32 144, i32 158, i32 25, i32 97, i32 91, i32 21, i32 171, i32 54,
	i32 127, i32 89, i32 128, i32 75, i32 118, i32 127, i32 143, i32 87,
	i32 69, i32 8, i32 169, i32 9, i32 42, i32 64, i32 167, i32 12,
	i32 155, i32 97, i32 135, i32 18, i32 100, i32 166, i32 27, i32 43,
	i32 170, i32 89, i32 86, i32 16, i32 49, i32 165, i32 111, i32 135,
	i32 118, i32 112, i32 152, i32 142, i32 27, i32 159, i32 108, i32 73,
	i32 151, i32 8, i32 94, i32 58, i32 95, i32 50, i32 13, i32 11,
	i32 94, i32 169, i32 130, i32 44, i32 62, i32 29, i32 129, i32 114,
	i32 7, i32 157, i32 117, i32 33, i32 20, i32 143, i32 140, i32 160,
	i32 26, i32 156, i32 5, i32 112, i32 163, i32 45, i32 77, i32 34,
	i32 70, i32 115, i32 8, i32 163, i32 101, i32 6, i32 134, i32 53,
	i32 2, i32 51, i32 92, i32 38, i32 143, i32 120, i32 145, i32 101,
	i32 75, i32 128, i32 63, i32 91, i32 1, i32 56, i32 154, i32 95,
	i32 69, i32 162, i32 73, i32 58, i32 83, i32 63, i32 68, i32 167,
	i32 171, i32 20, i32 99, i32 151, i32 95, i32 114, i32 24, i32 162,
	i32 22, i32 120, i32 138, i32 86, i32 156, i32 125, i32 82, i32 129,
	i32 122, i32 141, i32 148, i32 14, i32 82, i32 170, i32 158, i32 1,
	i32 60, i32 51, i32 37, i32 80, i32 116, i32 130, i32 109, i32 73,
	i32 55, i32 25, i32 129, i32 146, i32 31, i32 152, i32 78, i32 103,
	i32 98, i32 139, i32 59, i32 168, i32 110, i32 15, i32 41, i32 99,
	i32 72, i32 160, i32 107, i32 3, i32 46, i32 111, i32 136, i32 147,
	i32 71, i32 103, i32 155, i32 105, i32 163, i32 109, i32 60, i32 5,
	i32 41, i32 96, i32 124, i32 52, i32 4, i32 148, i32 168, i32 101,
	i32 93, i32 35, i32 51, i32 149, i32 108, i32 80, i32 74, i32 3,
	i32 115, i32 117, i32 9, i32 60, i32 147, i32 18, i32 65, i32 55,
	i32 50, i32 74, i32 50, i32 84, i32 53, i32 2, i32 28, i32 18,
	i32 44, i32 14, i32 105, i32 11, i32 124, i32 43, i32 38, i32 88,
	i32 149, i32 40, i32 17, i32 27, i32 77, i32 45, i32 7, i32 106,
	i32 25, i32 4, i32 135, i32 36, i32 17, i32 137, i32 48, i32 104,
	i32 138, i32 107, i32 91, i32 39, i32 79, i32 144, i32 61, i32 166,
	i32 33, i32 67, i32 70, i32 116, i32 29, i32 57, i32 32, i32 33,
	i32 38, i32 159, i32 118, i32 54, i32 96, i32 167, i32 105, i32 146,
	i32 82, i32 110, i32 112, i32 9, i32 74, i32 160, i32 100, i32 56,
	i32 64, i32 83, i32 10, i32 23, i32 59, i32 22, i32 21, i32 59,
	i32 114, i32 34, i32 119, i32 80, i32 52, i32 75, i32 40, i32 156,
	i32 123, i32 1, i32 17, i32 119, i32 6, i32 13, i32 55, i32 107,
	i32 100, i32 122, i32 37, i32 85, i32 16, i32 66, i32 39, i32 19,
	i32 83, i32 79, i32 153, i32 94, i32 93, i32 86, i32 121, i32 16,
	i32 43, i32 37, i32 109, i32 150, i32 137, i32 153, i32 165, i32 88,
	i32 89, i32 76, i32 78, i32 12, i32 36, i32 47, i32 141, i32 126,
	i32 42, i32 5, i32 122, i32 149, i32 79, i32 164, i32 23, i32 158,
	i32 19, i32 161, i32 106, i32 133, i32 132, i32 169, i32 138, i32 57,
	i32 81, i32 26, i32 157, i32 3, i32 65, i32 70, i32 10, i32 0,
	i32 121, i32 111, i32 47, i32 115, i32 26, i32 168, i32 45, i32 133,
	i32 99, i32 22, i32 15, i32 164, i32 104, i32 134, i32 150, i32 61,
	i32 126, i32 92, i32 69, i32 68, i32 125, i32 0, i32 108, i32 123,
	i32 66, i32 15, i32 92, i32 81, i32 106, i32 131, i32 84, i32 141,
	i32 139, i32 154, i32 161, i32 103, i32 113, i32 35, i32 28, i32 20,
	i32 98, i32 23, i32 34, i32 157, i32 96, i32 97, i32 32, i32 98,
	i32 131, i32 90, i32 110, i32 127, i32 140, i32 144, i32 113, i32 78,
	i32 44, i32 65, i32 147, i32 68, i32 139, i32 46, i32 14, i32 42,
	i32 81, i32 48, i32 88, i32 90, i32 39, i32 165, i32 47, i32 41,
	i32 102, i32 30, i32 142, i32 30, i32 123, i32 46, i32 152, i32 155,
	i32 32, i32 164, i32 11, i32 67, i32 72, i32 121, i32 148, i32 140,
	i32 132, i32 113, i32 131, i32 13, i32 21, i32 57, i32 12, i32 7,
	i32 151, i32 171, i32 63, i32 116, i32 64, i32 134, i32 124, i32 85,
	i32 71, i32 54, i32 66, i32 6, i32 128, i32 133, i32 136, i32 52,
	i32 117, i32 19, i32 93, i32 166, i32 142, i32 119, i32 136, i32 72,
	i32 40, i32 4, i32 137, i32 31, i32 56, i32 29, i32 145, i32 87,
	i32 161, i32 10, i32 49, i32 62, i32 132, i32 0, i32 159, i32 58,
	i32 126, i32 150, i32 84, i32 120, i32 76, i32 146, i32 154, i32 77,
	i32 35, i32 28, i32 62, i32 125
], align 16

@marshal_methods_number_of_classes = dso_local local_unnamed_addr constant i32 0, align 4

@marshal_methods_class_cache = dso_local local_unnamed_addr global [0 x %struct.MarshalMethodsManagedClass] zeroinitializer, align 8

; Names of classes in which marshal methods reside
@mm_class_names = dso_local local_unnamed_addr constant [0 x ptr] zeroinitializer, align 8

@mm_method_names = dso_local local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	%struct.MarshalMethodName {
		i64 u0x0000000000000000, ; name: 
		ptr @.MarshalMethodName.0_name; char* name
	} ; 0
], align 8

; get_function_pointer (uint32_t mono_image_index, uint32_t class_index, uint32_t method_token, void*& target_ptr)
@get_function_pointer = internal dso_local unnamed_addr global ptr null, align 8

; Functions

; Function attributes: memory(write, argmem: none, inaccessiblemem: none) "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" uwtable willreturn
define void @xamarin_app_init(ptr nocapture noundef readnone %env, ptr noundef %fn) local_unnamed_addr #0
{
	%fnIsNull = icmp eq ptr %fn, null
	br i1 %fnIsNull, label %1, label %2

1: ; preds = %0
	%putsResult = call noundef i32 @puts(ptr @.str.0)
	call void @abort()
	unreachable 

2: ; preds = %1, %0
	store ptr %fn, ptr @get_function_pointer, align 8, !tbaa !3
	ret void
}

; Strings
@.str.0 = private unnamed_addr constant [40 x i8] c"get_function_pointer MUST be specified\0A\00", align 16

;MarshalMethodName
@.MarshalMethodName.0_name = private unnamed_addr constant [1 x i8] c"\00", align 1

; External functions

; Function attributes: noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8"
declare void @abort() local_unnamed_addr #2

; Function attributes: nofree nounwind
declare noundef i32 @puts(ptr noundef) local_unnamed_addr #1
attributes #0 = { memory(write, argmem: none, inaccessiblemem: none) "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+crc32,+cx16,+cx8,+fxsr,+mmx,+popcnt,+sse,+sse2,+sse3,+sse4.1,+sse4.2,+ssse3,+x87" "tune-cpu"="generic" uwtable willreturn }
attributes #1 = { nofree nounwind }
attributes #2 = { noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+crc32,+cx16,+cx8,+fxsr,+mmx,+popcnt,+sse,+sse2,+sse3,+sse4.1,+sse4.2,+ssse3,+x87" "tune-cpu"="generic" }

; Metadata
!llvm.module.flags = !{!0, !1}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!llvm.ident = !{!2}
!2 = !{!".NET for Android remotes/origin/release/9.0.1xx @ 278e101698269c9bc8840aa94d72e7f24066a96d"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C++ TBAA"}
