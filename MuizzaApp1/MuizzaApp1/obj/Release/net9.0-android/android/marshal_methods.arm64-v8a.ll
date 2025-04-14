; ModuleID = 'marshal_methods.arm64-v8a.ll'
source_filename = "marshal_methods.arm64-v8a.ll"
target datalayout = "e-m:e-i8:8:32-i16:16:32-i64:64-i128:128-n32:64-S128"
target triple = "aarch64-unknown-linux-android21"

%struct.MarshalMethodName = type {
	i64, ; uint64_t id
	ptr ; char* name
}

%struct.MarshalMethodsManagedClass = type {
	i32, ; uint32_t token
	ptr ; MonoClass klass
}

@assembly_image_cache = dso_local local_unnamed_addr global [163 x ptr] zeroinitializer, align 8

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [489 x i64] [
	i64 u0x0071cf2d27b7d61e, ; 0: lib_Xamarin.AndroidX.SwipeRefreshLayout.dll.so => 87
	i64 u0x02123411c4e01926, ; 1: lib_Xamarin.AndroidX.Navigation.Runtime.dll.so => 83
	i64 u0x022e81ea9c46e03a, ; 2: lib_CommunityToolkit.Maui.Core.dll.so => 36
	i64 u0x02abedc11addc1ed, ; 3: lib_Mono.Android.Runtime.dll.so => 161
	i64 u0x032267b2a94db371, ; 4: lib_Xamarin.AndroidX.AppCompat.dll.so => 66
	i64 u0x0363ac97a4cb84e6, ; 5: SQLitePCLRaw.provider.e_sqlite3.dll => 64
	i64 u0x043032f1d071fae0, ; 6: ru/Microsoft.Maui.Controls.resources => 24
	i64 u0x044440a55165631e, ; 7: lib-cs-Microsoft.Maui.Controls.resources.dll.so => 2
	i64 u0x046eb1581a80c6b0, ; 8: vi/Microsoft.Maui.Controls.resources => 30
	i64 u0x0517ef04e06e9f76, ; 9: System.Net.Primitives => 127
	i64 u0x051a3be159e4ef99, ; 10: Xamarin.GooglePlayServices.Tasks => 95
	i64 u0x0565d18c6da3de38, ; 11: Xamarin.AndroidX.RecyclerView => 85
	i64 u0x0581db89237110e9, ; 12: lib_System.Collections.dll.so => 105
	i64 u0x05989cb940b225a9, ; 13: Microsoft.Maui.dll => 54
	i64 u0x06076b5d2b581f08, ; 14: zh-HK/Microsoft.Maui.Controls.resources => 31
	i64 u0x06388ffe9f6c161a, ; 15: System.Xml.Linq.dll => 155
	i64 u0x0680a433c781bb3d, ; 16: Xamarin.AndroidX.Collection.Jvm => 69
	i64 u0x07c57877c7ba78ad, ; 17: ru/Microsoft.Maui.Controls.resources.dll => 24
	i64 u0x07dcdc7460a0c5e4, ; 18: System.Collections.NonGeneric => 103
	i64 u0x08122e52765333c8, ; 19: lib_Microsoft.Extensions.Logging.Debug.dll.so => 49
	i64 u0x08f3c9788ee2153c, ; 20: Xamarin.AndroidX.DrawerLayout => 74
	i64 u0x0919c28b89381a0b, ; 21: lib_Microsoft.Extensions.Options.dll.so => 50
	i64 u0x092266563089ae3e, ; 22: lib_System.Collections.NonGeneric.dll.so => 103
	i64 u0x098b50f911ccea8d, ; 23: lib_Xamarin.GooglePlayServices.Basement.dll.so => 93
	i64 u0x09d144a7e214d457, ; 24: System.Security.Cryptography => 145
	i64 u0x0abb3e2b271edc45, ; 25: System.Threading.Channels.dll => 150
	i64 u0x0b3b632c3bbee20c, ; 26: sk/Microsoft.Maui.Controls.resources => 25
	i64 u0x0b6aff547b84fbe9, ; 27: Xamarin.KotlinX.Serialization.Core.Jvm => 98
	i64 u0x0be2e1f8ce4064ed, ; 28: Xamarin.AndroidX.ViewPager => 89
	i64 u0x0c3ca6cc978e2aae, ; 29: pt-BR/Microsoft.Maui.Controls.resources => 21
	i64 u0x0c59ad9fbbd43abe, ; 30: Mono.Android => 162
	i64 u0x0c7790f60165fc06, ; 31: lib_Microsoft.Maui.Essentials.dll.so => 55
	i64 u0x0c83c82812e96127, ; 32: lib_System.Net.Mail.dll.so => 124
	i64 u0x0da13792b76f89bf, ; 33: lib_Microsoft.Extensions.Configuration.UserSecrets.dll.so => 41
	i64 u0x0e14e73a54dda68e, ; 34: lib_System.Net.NameResolution.dll.so => 125
	i64 u0x102a31b45304b1da, ; 35: Xamarin.AndroidX.CustomView => 73
	i64 u0x10f6cfcbcf801616, ; 36: System.IO.Compression.Brotli => 116
	i64 u0x114443cdcf2091f1, ; 37: System.Security.Cryptography.Primitives => 144
	i64 u0x11a603952763e1d4, ; 38: System.Net.Mail => 124
	i64 u0x125b7f94acb989db, ; 39: Xamarin.AndroidX.RecyclerView.dll => 85
	i64 u0x13a01de0cbc3f06c, ; 40: lib-fr-Microsoft.Maui.Controls.resources.dll.so => 8
	i64 u0x13f1e5e209e91af4, ; 41: lib_Java.Interop.dll.so => 160
	i64 u0x13f1e880c25d96d1, ; 42: he/Microsoft.Maui.Controls.resources => 9
	i64 u0x143d8ea60a6a4011, ; 43: Microsoft.Extensions.DependencyInjection.Abstractions => 43
	i64 u0x16ea2b318ad2d830, ; 44: System.Security.Cryptography.Algorithms => 143
	i64 u0x17125c9a85b4929f, ; 45: lib_netstandard.dll.so => 158
	i64 u0x17b56e25558a5d36, ; 46: lib-hu-Microsoft.Maui.Controls.resources.dll.so => 12
	i64 u0x17f9358913beb16a, ; 47: System.Text.Encodings.Web => 147
	i64 u0x18402a709e357f3b, ; 48: lib_Xamarin.KotlinX.Serialization.Core.Jvm.dll.so => 98
	i64 u0x18f0ce884e87d89a, ; 49: nb/Microsoft.Maui.Controls.resources.dll => 18
	i64 u0x1a91866a319e9259, ; 50: lib_System.Collections.Concurrent.dll.so => 101
	i64 u0x1aac34d1917ba5d3, ; 51: lib_System.dll.so => 157
	i64 u0x1aad60783ffa3e5b, ; 52: lib-th-Microsoft.Maui.Controls.resources.dll.so => 27
	i64 u0x1c292b1598348d77, ; 53: Microsoft.Extensions.Diagnostics.dll => 44
	i64 u0x1c753b5ff15bce1b, ; 54: Mono.Android.Runtime.dll => 161
	i64 u0x1e3d87657e9659bc, ; 55: Xamarin.AndroidX.Navigation.UI => 84
	i64 u0x1e71143913d56c10, ; 56: lib-ko-Microsoft.Maui.Controls.resources.dll.so => 16
	i64 u0x1e7c31185e2fb266, ; 57: lib_System.Threading.Tasks.Parallel.dll.so => 151
	i64 u0x1ed8fcce5e9b50a0, ; 58: Microsoft.Extensions.Options.dll => 50
	i64 u0x209375905fcc1bad, ; 59: lib_System.IO.Compression.Brotli.dll.so => 116
	i64 u0x2174319c0d835bc9, ; 60: System.Runtime => 142
	i64 u0x220fd4f2e7c48170, ; 61: th/Microsoft.Maui.Controls.resources => 27
	i64 u0x2347c268e3e4e536, ; 62: Xamarin.GooglePlayServices.Basement.dll => 93
	i64 u0x237be844f1f812c7, ; 63: System.Threading.Thread.dll => 152
	i64 u0x2407aef2bbe8fadf, ; 64: System.Console => 109
	i64 u0x240abe014b27e7d3, ; 65: Xamarin.AndroidX.Core.dll => 71
	i64 u0x247619fe4413f8bf, ; 66: System.Runtime.Serialization.Primitives.dll => 141
	i64 u0x252073cc3caa62c2, ; 67: fr/Microsoft.Maui.Controls.resources.dll => 8
	i64 u0x25a0a7eff76ea08e, ; 68: SQLitePCLRaw.batteries_v2.dll => 61
	i64 u0x2662c629b96b0b30, ; 69: lib_Xamarin.Kotlin.StdLib.dll.so => 96
	i64 u0x268c1439f13bcc29, ; 70: lib_Microsoft.Extensions.Primitives.dll.so => 51
	i64 u0x273f3515de5faf0d, ; 71: id/Microsoft.Maui.Controls.resources.dll => 13
	i64 u0x2742545f9094896d, ; 72: hr/Microsoft.Maui.Controls.resources => 11
	i64 u0x27b410442fad6cf1, ; 73: Java.Interop.dll => 160
	i64 u0x2801845a2c71fbfb, ; 74: System.Net.Primitives.dll => 127
	i64 u0x28e52865585a1ebe, ; 75: Microsoft.Extensions.Diagnostics.Abstractions.dll => 45
	i64 u0x2a128783efe70ba0, ; 76: uk/Microsoft.Maui.Controls.resources.dll => 29
	i64 u0x2a3b095612184159, ; 77: lib_System.Net.NetworkInformation.dll.so => 126
	i64 u0x2a6507a5ffabdf28, ; 78: System.Diagnostics.TraceSource.dll => 112
	i64 u0x2ad156c8e1354139, ; 79: fi/Microsoft.Maui.Controls.resources => 7
	i64 u0x2af298f63581d886, ; 80: System.Text.RegularExpressions.dll => 149
	i64 u0x2afc1c4f898552ee, ; 81: lib_System.Formats.Asn1.dll.so => 115
	i64 u0x2b148910ed40fbf9, ; 82: zh-Hant/Microsoft.Maui.Controls.resources.dll => 33
	i64 u0x2c8bd14bb93a7d82, ; 83: lib-pl-Microsoft.Maui.Controls.resources.dll.so => 20
	i64 u0x2cd723e9fe623c7c, ; 84: lib_System.Private.Xml.Linq.dll.so => 134
	i64 u0x2d169d318a968379, ; 85: System.Threading.dll => 153
	i64 u0x2d47774b7d993f59, ; 86: sv/Microsoft.Maui.Controls.resources.dll => 26
	i64 u0x2db915caf23548d2, ; 87: System.Text.Json.dll => 148
	i64 u0x2e6f1f226821322a, ; 88: el/Microsoft.Maui.Controls.resources.dll => 5
	i64 u0x2f2e98e1c89b1aff, ; 89: System.Xml.ReaderWriter => 156
	i64 u0x2ff49de6a71764a1, ; 90: lib_Microsoft.Extensions.Http.dll.so => 46
	i64 u0x309ee9eeec09a71e, ; 91: lib_Xamarin.AndroidX.Fragment.dll.so => 75
	i64 u0x31195fef5d8fb552, ; 92: _Microsoft.Android.Resource.Designer.dll => 34
	i64 u0x32243413e774362a, ; 93: Xamarin.AndroidX.CardView.dll => 68
	i64 u0x3235427f8d12dae1, ; 94: lib_System.Drawing.Primitives.dll.so => 113
	i64 u0x329753a17a517811, ; 95: fr/Microsoft.Maui.Controls.resources => 8
	i64 u0x32aa989ff07a84ff, ; 96: lib_System.Xml.ReaderWriter.dll.so => 156
	i64 u0x33829542f112d59b, ; 97: System.Collections.Immutable => 102
	i64 u0x33a31443733849fe, ; 98: lib-es-Microsoft.Maui.Controls.resources.dll.so => 6
	i64 u0x341abc357fbb4ebf, ; 99: lib_System.Net.Sockets.dll.so => 130
	i64 u0x34dfd74fe2afcf37, ; 100: Microsoft.Maui => 54
	i64 u0x34e292762d9615df, ; 101: cs/Microsoft.Maui.Controls.resources.dll => 2
	i64 u0x3508234247f48404, ; 102: Microsoft.Maui.Controls => 52
	i64 u0x3549870798b4cd30, ; 103: lib_Xamarin.AndroidX.ViewPager2.dll.so => 90
	i64 u0x355282fc1c909694, ; 104: Microsoft.Extensions.Configuration => 38
	i64 u0x380134e03b1e160a, ; 105: System.Collections.Immutable.dll => 102
	i64 u0x385c17636bb6fe6e, ; 106: Xamarin.AndroidX.CustomView.dll => 73
	i64 u0x38869c811d74050e, ; 107: System.Net.NameResolution.dll => 125
	i64 u0x3889cbdca0f2c57c, ; 108: Xamarin.GooglePlayServices.Location.dll => 94
	i64 u0x393c226616977fdb, ; 109: lib_Xamarin.AndroidX.ViewPager.dll.so => 89
	i64 u0x395e37c3334cf82a, ; 110: lib-ca-Microsoft.Maui.Controls.resources.dll.so => 1
	i64 u0x39aa39fda111d9d3, ; 111: Newtonsoft.Json => 58
	i64 u0x3ab5859054645f72, ; 112: System.Security.Cryptography.Primitives.dll => 144
	i64 u0x3b860f9932505633, ; 113: lib_System.Text.Encoding.Extensions.dll.so => 146
	i64 u0x3c7c495f58ac5ee9, ; 114: Xamarin.Kotlin.StdLib => 96
	i64 u0x3d1f54d6b217cd0f, ; 115: Microsoft.Extensions.Configuration.UserSecrets => 41
	i64 u0x3d46f0b995082740, ; 116: System.Xml.Linq => 155
	i64 u0x3d9c2a242b040a50, ; 117: lib_Xamarin.AndroidX.Core.dll.so => 71
	i64 u0x3da7781d6333a8fe, ; 118: SQLitePCLRaw.batteries_v2 => 61
	i64 u0x407a10bb4bf95829, ; 119: lib_Xamarin.AndroidX.Navigation.Common.dll.so => 81
	i64 u0x41cab042be111c34, ; 120: lib_Xamarin.AndroidX.AppCompat.AppCompatResources.dll.so => 67
	i64 u0x43375950ec7c1b6a, ; 121: netstandard.dll => 158
	i64 u0x434c4e1d9284cdae, ; 122: Mono.Android.dll => 162
	i64 u0x43950f84de7cc79a, ; 123: pl/Microsoft.Maui.Controls.resources.dll => 20
	i64 u0x448bd33429269b19, ; 124: Microsoft.CSharp => 100
	i64 u0x4499fa3c8e494654, ; 125: lib_System.Runtime.Serialization.Primitives.dll.so => 141
	i64 u0x4515080865a951a5, ; 126: Xamarin.Kotlin.StdLib.dll => 96
	i64 u0x45c40276a42e283e, ; 127: System.Diagnostics.TraceSource => 112
	i64 u0x46a4213bc97fe5ae, ; 128: lib-ru-Microsoft.Maui.Controls.resources.dll.so => 24
	i64 u0x47358bd471172e1d, ; 129: lib_System.Xml.Linq.dll.so => 155
	i64 u0x47daf4e1afbada10, ; 130: pt/Microsoft.Maui.Controls.resources => 22
	i64 u0x49e952f19a4e2022, ; 131: System.ObjectModel => 132
	i64 u0x49f9e6948a8131e4, ; 132: lib_Xamarin.AndroidX.VersionedParcelable.dll.so => 88
	i64 u0x4a5667b2462a664b, ; 133: lib_Xamarin.AndroidX.Navigation.UI.dll.so => 84
	i64 u0x4b7b6532ded934b7, ; 134: System.Text.Json => 148
	i64 u0x4c7755cf07ad2d5f, ; 135: System.Net.Http.Json.dll => 122
	i64 u0x4cc5f15266470798, ; 136: lib_Xamarin.AndroidX.Loader.dll.so => 80
	i64 u0x4cf6f67dc77aacd2, ; 137: System.Net.NetworkInformation.dll => 126
	i64 u0x4d479f968a05e504, ; 138: System.Linq.Expressions.dll => 119
	i64 u0x4d55a010ffc4faff, ; 139: System.Private.Xml => 135
	i64 u0x4d95fccc1f67c7ca, ; 140: System.Runtime.Loader.dll => 138
	i64 u0x4dcf44c3c9b076a2, ; 141: it/Microsoft.Maui.Controls.resources.dll => 14
	i64 u0x4dd9247f1d2c3235, ; 142: Xamarin.AndroidX.Loader.dll => 80
	i64 u0x4de0a6be5cce1b0e, ; 143: lib_NBitcoin.dll.so => 57
	i64 u0x4e32f00cb0937401, ; 144: Mono.Android.Runtime => 161
	i64 u0x4ebd0c4b82c5eefc, ; 145: lib_System.Threading.Channels.dll.so => 150
	i64 u0x4f21ee6ef9eb527e, ; 146: ca/Microsoft.Maui.Controls.resources => 1
	i64 u0x4fd5f3ee53d0a4f0, ; 147: SQLitePCLRaw.lib.e_sqlite3.android => 63
	i64 u0x5037f0be3c28c7a3, ; 148: lib_Microsoft.Maui.Controls.dll.so => 52
	i64 u0x5112ed116d87baf8, ; 149: CommunityToolkit.Mvvm => 37
	i64 u0x5131bbe80989093f, ; 150: Xamarin.AndroidX.Lifecycle.ViewModel.Android.dll => 78
	i64 u0x51bb8a2afe774e32, ; 151: System.Drawing => 114
	i64 u0x526ce79eb8e90527, ; 152: lib_System.Net.Primitives.dll.so => 127
	i64 u0x52829f00b4467c38, ; 153: lib_System.Data.Common.dll.so => 110
	i64 u0x529ffe06f39ab8db, ; 154: Xamarin.AndroidX.Core => 71
	i64 u0x52ff996554dbf352, ; 155: Microsoft.Maui.Graphics => 56
	i64 u0x535f7e40e8fef8af, ; 156: lib-sk-Microsoft.Maui.Controls.resources.dll.so => 25
	i64 u0x53a96d5c86c9e194, ; 157: System.Net.NetworkInformation => 126
	i64 u0x53be1038a61e8d44, ; 158: System.Runtime.InteropServices.RuntimeInformation.dll => 136
	i64 u0x53c3014b9437e684, ; 159: lib-zh-HK-Microsoft.Maui.Controls.resources.dll.so => 31
	i64 u0x54795225dd1587af, ; 160: lib_System.Runtime.dll.so => 142
	i64 u0x556e8b63b660ab8b, ; 161: Xamarin.AndroidX.Lifecycle.Common.Jvm.dll => 76
	i64 u0x5588627c9a108ec9, ; 162: System.Collections.Specialized => 104
	i64 u0x55c62636751e5d4d, ; 163: MuizzaApp1.dll => 99
	i64 u0x571c5cfbec5ae8e2, ; 164: System.Private.Uri => 133
	i64 u0x578cd35c91d7b347, ; 165: lib_SQLitePCLRaw.core.dll.so => 62
	i64 u0x579a06fed6eec900, ; 166: System.Private.CoreLib.dll => 159
	i64 u0x57c542c14049b66d, ; 167: System.Diagnostics.DiagnosticSource => 111
	i64 u0x58601b2dda4a27b9, ; 168: lib-ja-Microsoft.Maui.Controls.resources.dll.so => 15
	i64 u0x58688d9af496b168, ; 169: Microsoft.Extensions.DependencyInjection.dll => 42
	i64 u0x595a356d23e8da9a, ; 170: lib_Microsoft.CSharp.dll.so => 100
	i64 u0x5a89a886ae30258d, ; 171: lib_Xamarin.AndroidX.CoordinatorLayout.dll.so => 70
	i64 u0x5a8f6699f4a1caa9, ; 172: lib_System.Threading.dll.so => 153
	i64 u0x5ae9cd33b15841bf, ; 173: System.ComponentModel => 108
	i64 u0x5b5f0e240a06a2a2, ; 174: da/Microsoft.Maui.Controls.resources.dll => 3
	i64 u0x5b755276902c8414, ; 175: Xamarin.GooglePlayServices.Base => 92
	i64 u0x5c393624b8176517, ; 176: lib_Microsoft.Extensions.Logging.dll.so => 47
	i64 u0x5d7ec76c1c703055, ; 177: System.Threading.Tasks.Parallel => 151
	i64 u0x5db0cbbd1028510e, ; 178: lib_System.Runtime.InteropServices.dll.so => 137
	i64 u0x5db30905d3e5013b, ; 179: Xamarin.AndroidX.Collection.Jvm.dll => 69
	i64 u0x5e467bc8f09ad026, ; 180: System.Collections.Specialized.dll => 104
	i64 u0x5ea92fdb19ec8c4c, ; 181: System.Text.Encodings.Web.dll => 147
	i64 u0x5eb8046dd40e9ac3, ; 182: System.ComponentModel.Primitives => 106
	i64 u0x5f36ccf5c6a57e24, ; 183: System.Xml.ReaderWriter.dll => 156
	i64 u0x5f4294b9b63cb842, ; 184: System.Data.Common => 110
	i64 u0x5f7399e166075632, ; 185: lib_SQLitePCLRaw.lib.e_sqlite3.android.dll.so => 63
	i64 u0x5f9a2d823f664957, ; 186: lib-el-Microsoft.Maui.Controls.resources.dll.so => 5
	i64 u0x609f4b7b63d802d4, ; 187: lib_Microsoft.Extensions.DependencyInjection.dll.so => 42
	i64 u0x60cd4e33d7e60134, ; 188: Xamarin.KotlinX.Coroutines.Core.Jvm => 97
	i64 u0x60f62d786afcf130, ; 189: System.Memory => 121
	i64 u0x61be8d1299194243, ; 190: Microsoft.Maui.Controls.Xaml => 53
	i64 u0x61d2cba29557038f, ; 191: de/Microsoft.Maui.Controls.resources => 4
	i64 u0x61d88f399afb2f45, ; 192: lib_System.Runtime.Loader.dll.so => 138
	i64 u0x622eef6f9e59068d, ; 193: System.Private.CoreLib => 159
	i64 u0x63f1f6883c1e23c2, ; 194: lib_System.Collections.Immutable.dll.so => 102
	i64 u0x6400f68068c1e9f1, ; 195: Xamarin.Google.Android.Material.dll => 91
	i64 u0x640e3b14dbd325c2, ; 196: System.Security.Cryptography.Algorithms.dll => 143
	i64 u0x658f524e4aba7dad, ; 197: CommunityToolkit.Maui.dll => 35
	i64 u0x65ecac39144dd3cc, ; 198: Microsoft.Maui.Controls.dll => 52
	i64 u0x65ece51227bfa724, ; 199: lib_System.Runtime.Numerics.dll.so => 139
	i64 u0x6692e924eade1b29, ; 200: lib_System.Console.dll.so => 109
	i64 u0x66a4e5c6a3fb0bae, ; 201: lib_Xamarin.AndroidX.Lifecycle.ViewModel.Android.dll.so => 78
	i64 u0x66d13304ce1a3efa, ; 202: Xamarin.AndroidX.CursorAdapter => 72
	i64 u0x68558ec653afa616, ; 203: lib-da-Microsoft.Maui.Controls.resources.dll.so => 3
	i64 u0x6872ec7a2e36b1ac, ; 204: System.Drawing.Primitives.dll => 113
	i64 u0x68fbbbe2eb455198, ; 205: System.Formats.Asn1 => 115
	i64 u0x69063fc0ba8e6bdd, ; 206: he/Microsoft.Maui.Controls.resources.dll => 9
	i64 u0x699dffb2427a2d71, ; 207: SQLitePCLRaw.lib.e_sqlite3.android.dll => 63
	i64 u0x6a4d7577b2317255, ; 208: System.Runtime.InteropServices.dll => 137
	i64 u0x6ace3b74b15ee4a4, ; 209: nb/Microsoft.Maui.Controls.resources => 18
	i64 u0x6c3292dfcd2f6999, ; 210: NBitcoin.dll => 57
	i64 u0x6d12bfaa99c72b1f, ; 211: lib_Microsoft.Maui.Graphics.dll.so => 56
	i64 u0x6d79993361e10ef2, ; 212: Microsoft.Extensions.Primitives => 51
	i64 u0x6d86d56b84c8eb71, ; 213: lib_Xamarin.AndroidX.CursorAdapter.dll.so => 72
	i64 u0x6d9bea6b3e895cf7, ; 214: Microsoft.Extensions.Primitives.dll => 51
	i64 u0x6e25a02c3833319a, ; 215: lib_Xamarin.AndroidX.Navigation.Fragment.dll.so => 82
	i64 u0x6fd2265da78b93a4, ; 216: lib_Microsoft.Maui.dll.so => 54
	i64 u0x6fdfc7de82c33008, ; 217: cs/Microsoft.Maui.Controls.resources => 2
	i64 u0x70e99f48c05cb921, ; 218: tr/Microsoft.Maui.Controls.resources.dll => 28
	i64 u0x70fd3deda22442d2, ; 219: lib-nb-Microsoft.Maui.Controls.resources.dll.so => 18
	i64 u0x717530326f808838, ; 220: lib_Microsoft.Extensions.Diagnostics.Abstractions.dll.so => 45
	i64 u0x71a495ea3761dde8, ; 221: lib-it-Microsoft.Maui.Controls.resources.dll.so => 14
	i64 u0x71ad672adbe48f35, ; 222: System.ComponentModel.Primitives.dll => 106
	i64 u0x72b1fb4109e08d7b, ; 223: lib-hr-Microsoft.Maui.Controls.resources.dll.so => 11
	i64 u0x73e4ce94e2eb6ffc, ; 224: lib_System.Memory.dll.so => 121
	i64 u0x746cf89b511b4d40, ; 225: lib_Microsoft.Extensions.Diagnostics.dll.so => 44
	i64 u0x74fcb5b9d3ee6884, ; 226: Plugin.LocalNotification => 59
	i64 u0x755a91767330b3d4, ; 227: lib_Microsoft.Extensions.Configuration.dll.so => 38
	i64 u0x76012e7334db86e5, ; 228: lib_Xamarin.AndroidX.SavedState.dll.so => 86
	i64 u0x76ca07b878f44da0, ; 229: System.Runtime.Numerics.dll => 139
	i64 u0x779f67ad3b8efbd5, ; 230: Microsoft.Extensions.Configuration.Json.dll => 40
	i64 u0x780bc73597a503a9, ; 231: lib-ms-Microsoft.Maui.Controls.resources.dll.so => 17
	i64 u0x783606d1e53e7a1a, ; 232: th/Microsoft.Maui.Controls.resources.dll => 27
	i64 u0x78a45e51311409b6, ; 233: Xamarin.AndroidX.Fragment.dll => 75
	i64 u0x7a090e7cbb6c0ed1, ; 234: Xamarin.GooglePlayServices.Location => 94
	i64 u0x7a25bdb29108c6e7, ; 235: Microsoft.Extensions.Http => 46
	i64 u0x7adb8da2ac89b647, ; 236: fi/Microsoft.Maui.Controls.resources.dll => 7
	i64 u0x7bef86a4335c4870, ; 237: System.ComponentModel.TypeConverter => 107
	i64 u0x7c0820144cd34d6a, ; 238: sk/Microsoft.Maui.Controls.resources.dll => 25
	i64 u0x7c2a0bd1e0f988fc, ; 239: lib-de-Microsoft.Maui.Controls.resources.dll.so => 4
	i64 u0x7cb95ad2a929d044, ; 240: Xamarin.GooglePlayServices.Basement => 93
	i64 u0x7cc637f941f716d0, ; 241: CommunityToolkit.Maui.Core => 36
	i64 u0x7d649b75d580bb42, ; 242: ms/Microsoft.Maui.Controls.resources.dll => 17
	i64 u0x7d8ee2bdc8e3aad1, ; 243: System.Numerics.Vectors => 131
	i64 u0x7df5df8db8eaa6ac, ; 244: Microsoft.Extensions.Logging.Debug => 49
	i64 u0x7dfc3d6d9d8d7b70, ; 245: System.Collections => 105
	i64 u0x7e946809d6008ef2, ; 246: lib_System.ObjectModel.dll.so => 132
	i64 u0x7eb4f0dc47488736, ; 247: lib_Xamarin.GooglePlayServices.Tasks.dll.so => 95
	i64 u0x7ecc13347c8fd849, ; 248: lib_System.ComponentModel.dll.so => 108
	i64 u0x7f00ddd9b9ca5a13, ; 249: Xamarin.AndroidX.ViewPager.dll => 89
	i64 u0x7f9351cd44b1273f, ; 250: Microsoft.Extensions.Configuration.Abstractions => 39
	i64 u0x7fbd557c99b3ce6f, ; 251: lib_Xamarin.AndroidX.Lifecycle.LiveData.Core.dll.so => 77
	i64 u0x80fa55b6d1b0be99, ; 252: SQLitePCLRaw.provider.e_sqlite3 => 64
	i64 u0x812c069d5cdecc17, ; 253: System.dll => 157
	i64 u0x81ab745f6c0f5ce6, ; 254: zh-Hant/Microsoft.Maui.Controls.resources => 33
	i64 u0x8277f2be6b5ce05f, ; 255: Xamarin.AndroidX.AppCompat => 66
	i64 u0x828f06563b30bc50, ; 256: lib_Xamarin.AndroidX.CardView.dll.so => 68
	i64 u0x82df8f5532a10c59, ; 257: lib_System.Drawing.dll.so => 114
	i64 u0x82f6403342e12049, ; 258: uk/Microsoft.Maui.Controls.resources => 29
	i64 u0x83144699b312ad81, ; 259: SQLite-net.dll => 60
	i64 u0x83c14ba66c8e2b8c, ; 260: zh-Hans/Microsoft.Maui.Controls.resources => 32
	i64 u0x846ce984efea52c7, ; 261: System.Threading.Tasks.Parallel.dll => 151
	i64 u0x86a909228dc7657b, ; 262: lib-zh-Hant-Microsoft.Maui.Controls.resources.dll.so => 33
	i64 u0x86b3e00c36b84509, ; 263: Microsoft.Extensions.Configuration.dll => 38
	i64 u0x87c69b87d9283884, ; 264: lib_System.Threading.Thread.dll.so => 152
	i64 u0x87f6569b25707834, ; 265: System.IO.Compression.Brotli.dll => 116
	i64 u0x8842b3a5d2d3fb36, ; 266: Microsoft.Maui.Essentials => 55
	i64 u0x88bda98e0cffb7a9, ; 267: lib_Xamarin.KotlinX.Coroutines.Core.Jvm.dll.so => 97
	i64 u0x8930322c7bd8f768, ; 268: netstandard => 158
	i64 u0x897a606c9e39c75f, ; 269: lib_System.ComponentModel.Primitives.dll.so => 106
	i64 u0x89c5188089ec2cd5, ; 270: lib_System.Runtime.InteropServices.RuntimeInformation.dll.so => 136
	i64 u0x8ad229ea26432ee2, ; 271: Xamarin.AndroidX.Loader => 80
	i64 u0x8b4ff5d0fdd5faa1, ; 272: lib_System.Diagnostics.DiagnosticSource.dll.so => 111
	i64 u0x8b9ceca7acae3451, ; 273: lib-he-Microsoft.Maui.Controls.resources.dll.so => 9
	i64 u0x8d0f420977c2c1c7, ; 274: Xamarin.AndroidX.CursorAdapter.dll => 72
	i64 u0x8d7b8ab4b3310ead, ; 275: System.Threading => 153
	i64 u0x8da188285aadfe8e, ; 276: System.Collections.Concurrent => 101
	i64 u0x8ec6e06a61c1baeb, ; 277: lib_Newtonsoft.Json.dll.so => 58
	i64 u0x8ed807bfe9858dfc, ; 278: Xamarin.AndroidX.Navigation.Common => 81
	i64 u0x8ee08b8194a30f48, ; 279: lib-hi-Microsoft.Maui.Controls.resources.dll.so => 10
	i64 u0x8ef7601039857a44, ; 280: lib-ro-Microsoft.Maui.Controls.resources.dll.so => 23
	i64 u0x8ef9414937d93a0a, ; 281: SQLitePCLRaw.core.dll => 62
	i64 u0x8efbc0801a122264, ; 282: Xamarin.GooglePlayServices.Tasks.dll => 95
	i64 u0x8f32c6f611f6ffab, ; 283: pt/Microsoft.Maui.Controls.resources.dll => 22
	i64 u0x8f8829d21c8985a4, ; 284: lib-pt-BR-Microsoft.Maui.Controls.resources.dll.so => 21
	i64 u0x8fd27d934d7b3a55, ; 285: SQLitePCLRaw.core => 62
	i64 u0x90263f8448b8f572, ; 286: lib_System.Diagnostics.TraceSource.dll.so => 112
	i64 u0x903101b46fb73a04, ; 287: _Microsoft.Android.Resource.Designer => 34
	i64 u0x90393bd4865292f3, ; 288: lib_System.IO.Compression.dll.so => 117
	i64 u0x90634f86c5ebe2b5, ; 289: Xamarin.AndroidX.Lifecycle.ViewModel.Android => 78
	i64 u0x907b636704ad79ef, ; 290: lib_Microsoft.Maui.Controls.Xaml.dll.so => 53
	i64 u0x91418dc638b29e68, ; 291: lib_Xamarin.AndroidX.CustomView.dll.so => 73
	i64 u0x914647982e998267, ; 292: Microsoft.Extensions.Configuration.Json => 40
	i64 u0x9157bd523cd7ed36, ; 293: lib_System.Text.Json.dll.so => 148
	i64 u0x91a74f07b30d37e2, ; 294: System.Linq.dll => 120
	i64 u0x91fa41a87223399f, ; 295: ca/Microsoft.Maui.Controls.resources.dll => 1
	i64 u0x93560decaaffea24, ; 296: NBitcoin => 57
	i64 u0x93cfa73ab28d6e35, ; 297: ms/Microsoft.Maui.Controls.resources => 17
	i64 u0x944077d8ca3c6580, ; 298: System.IO.Compression.dll => 117
	i64 u0x967fc325e09bfa8c, ; 299: es/Microsoft.Maui.Controls.resources => 6
	i64 u0x9732d8dbddea3d9a, ; 300: id/Microsoft.Maui.Controls.resources => 13
	i64 u0x978be80e5210d31b, ; 301: Microsoft.Maui.Graphics.dll => 56
	i64 u0x979ab54025cc1c7f, ; 302: lib_Xamarin.GooglePlayServices.Base.dll.so => 92
	i64 u0x97b8c771ea3e4220, ; 303: System.ComponentModel.dll => 108
	i64 u0x97e144c9d3c6976e, ; 304: System.Collections.Concurrent.dll => 101
	i64 u0x991d510397f92d9d, ; 305: System.Linq.Expressions => 119
	i64 u0x999cb19e1a04ffd3, ; 306: CommunityToolkit.Mvvm.dll => 37
	i64 u0x99a00ca5270c6878, ; 307: Xamarin.AndroidX.Navigation.Runtime => 83
	i64 u0x99cdc6d1f2d3a72f, ; 308: ko/Microsoft.Maui.Controls.resources.dll => 16
	i64 u0x9d5dbcf5a48583fe, ; 309: lib_Xamarin.AndroidX.Activity.dll.so => 65
	i64 u0x9d74dee1a7725f34, ; 310: Microsoft.Extensions.Configuration.Abstractions.dll => 39
	i64 u0x9e4534b6adaf6e84, ; 311: nl/Microsoft.Maui.Controls.resources => 19
	i64 u0x9eaf1efdf6f7267e, ; 312: Xamarin.AndroidX.Navigation.Common.dll => 81
	i64 u0x9ef542cf1f78c506, ; 313: Xamarin.AndroidX.Lifecycle.LiveData.Core => 77
	i64 u0x9fc2184212c417ad, ; 314: Plugin.LocalNotification.dll => 59
	i64 u0xa0d8259f4cc284ec, ; 315: lib_System.Security.Cryptography.dll.so => 145
	i64 u0xa1440773ee9d341e, ; 316: Xamarin.Google.Android.Material => 91
	i64 u0xa1b9d7c27f47219f, ; 317: Xamarin.AndroidX.Navigation.UI.dll => 84
	i64 u0xa2572680829d2c7c, ; 318: System.IO.Pipelines.dll => 118
	i64 u0xa46aa1eaa214539b, ; 319: ko/Microsoft.Maui.Controls.resources => 16
	i64 u0xa4a372eecb9e4df0, ; 320: Microsoft.Extensions.Diagnostics => 44
	i64 u0xa4d20d2ff0563d26, ; 321: lib_CommunityToolkit.Mvvm.dll.so => 37
	i64 u0xa4edc8f2ceae241a, ; 322: System.Data.Common.dll => 110
	i64 u0xa5494f40f128ce6a, ; 323: System.Runtime.Serialization.Formatters.dll => 140
	i64 u0xa5e599d1e0524750, ; 324: System.Numerics.Vectors.dll => 131
	i64 u0xa5f1ba49b85dd355, ; 325: System.Security.Cryptography.dll => 145
	i64 u0xa67dbee13e1df9ca, ; 326: Xamarin.AndroidX.SavedState.dll => 86
	i64 u0xa68a420042bb9b1f, ; 327: Xamarin.AndroidX.DrawerLayout.dll => 74
	i64 u0xa78ce3745383236a, ; 328: Xamarin.AndroidX.Lifecycle.Common.Jvm => 76
	i64 u0xa7c31b56b4dc7b33, ; 329: hu/Microsoft.Maui.Controls.resources => 12
	i64 u0xa843f6095f0d247d, ; 330: Xamarin.GooglePlayServices.Base.dll => 92
	i64 u0xa964304b5631e28a, ; 331: CommunityToolkit.Maui.Core.dll => 36
	i64 u0xaa2219c8e3449ff5, ; 332: Microsoft.Extensions.Logging.Abstractions => 48
	i64 u0xaa443ac34067eeef, ; 333: System.Private.Xml.dll => 135
	i64 u0xaa52de307ef5d1dd, ; 334: System.Net.Http => 123
	i64 u0xaaaf86367285a918, ; 335: Microsoft.Extensions.DependencyInjection.Abstractions.dll => 43
	i64 u0xaaf84bb3f052a265, ; 336: el/Microsoft.Maui.Controls.resources => 5
	i64 u0xab9c1b2687d86b0b, ; 337: lib_System.Linq.Expressions.dll.so => 119
	i64 u0xac2af3fa195a15ce, ; 338: System.Runtime.Numerics => 139
	i64 u0xac5376a2a538dc10, ; 339: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 77
	i64 u0xacd46e002c3ccb97, ; 340: ro/Microsoft.Maui.Controls.resources => 23
	i64 u0xacf42eea7ef9cd12, ; 341: System.Threading.Channels => 150
	i64 u0xad89c07347f1bad6, ; 342: nl/Microsoft.Maui.Controls.resources.dll => 19
	i64 u0xadbb53caf78a79d2, ; 343: System.Web.HttpUtility => 154
	i64 u0xadc90ab061a9e6e4, ; 344: System.ComponentModel.TypeConverter.dll => 107
	i64 u0xadf511667bef3595, ; 345: System.Net.Security => 129
	i64 u0xae282bcd03739de7, ; 346: Java.Interop => 160
	i64 u0xae53579c90db1107, ; 347: System.ObjectModel.dll => 132
	i64 u0xae7ea18c61eef394, ; 348: SQLite-net => 60
	i64 u0xafe29f45095518e7, ; 349: lib_Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll.so => 79
	i64 u0xb05cc42cd94c6d9d, ; 350: lib-sv-Microsoft.Maui.Controls.resources.dll.so => 26
	i64 u0xb220631954820169, ; 351: System.Text.RegularExpressions => 149
	i64 u0xb2a3f67f3bf29fce, ; 352: da/Microsoft.Maui.Controls.resources => 3
	i64 u0xb3f0a0fcda8d3ebc, ; 353: Xamarin.AndroidX.CardView => 68
	i64 u0xb46be1aa6d4fff93, ; 354: hi/Microsoft.Maui.Controls.resources => 10
	i64 u0xb477491be13109d8, ; 355: ar/Microsoft.Maui.Controls.resources => 0
	i64 u0xb4bd7015ecee9d86, ; 356: System.IO.Pipelines => 118
	i64 u0xb5c7fcdafbc67ee4, ; 357: Microsoft.Extensions.Logging.Abstractions.dll => 48
	i64 u0xb7212c4683a94afe, ; 358: System.Drawing.Primitives => 113
	i64 u0xb7b7753d1f319409, ; 359: sv/Microsoft.Maui.Controls.resources => 26
	i64 u0xb81a2c6e0aee50fe, ; 360: lib_System.Private.CoreLib.dll.so => 159
	i64 u0xb872c26142d22aa9, ; 361: Microsoft.Extensions.Http.dll => 46
	i64 u0xb9185c33a1643eed, ; 362: Microsoft.CSharp.dll => 100
	i64 u0xb9f64d3b230def68, ; 363: lib-pt-Microsoft.Maui.Controls.resources.dll.so => 22
	i64 u0xb9fc3c8a556e3691, ; 364: ja/Microsoft.Maui.Controls.resources => 15
	i64 u0xba48785529705af9, ; 365: System.Collections.dll => 105
	i64 u0xbb65706fde942ce3, ; 366: System.Net.Sockets => 130
	i64 u0xbbd180354b67271a, ; 367: System.Runtime.Serialization.Formatters => 140
	i64 u0xbc22a245dab70cb4, ; 368: lib_SQLitePCLRaw.provider.e_sqlite3.dll.so => 64
	i64 u0xbd0e2c0d55246576, ; 369: System.Net.Http.dll => 123
	i64 u0xbd437a2cdb333d0d, ; 370: Xamarin.AndroidX.ViewPager2 => 90
	i64 u0xbee38d4a88835966, ; 371: Xamarin.AndroidX.AppCompat.AppCompatResources => 67
	i64 u0xbfc1e1fb3095f2b3, ; 372: lib_System.Net.Http.Json.dll.so => 122
	i64 u0xc040a4ab55817f58, ; 373: ar/Microsoft.Maui.Controls.resources.dll => 0
	i64 u0xc0d928351ab5ca77, ; 374: System.Console.dll => 109
	i64 u0xc12b8b3afa48329c, ; 375: lib_System.Linq.dll.so => 120
	i64 u0xc1ff9ae3cdb6e1e6, ; 376: Xamarin.AndroidX.Activity.dll => 65
	i64 u0xc28c50f32f81cc73, ; 377: ja/Microsoft.Maui.Controls.resources.dll => 15
	i64 u0xc2bcfec99f69365e, ; 378: Xamarin.AndroidX.ViewPager2.dll => 90
	i64 u0xc4d3858ed4d08512, ; 379: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 79
	i64 u0xc50fded0ded1418c, ; 380: lib_System.ComponentModel.TypeConverter.dll.so => 107
	i64 u0xc519125d6bc8fb11, ; 381: lib_System.Net.Requests.dll.so => 128
	i64 u0xc5293b19e4dc230e, ; 382: Xamarin.AndroidX.Navigation.Fragment => 82
	i64 u0xc5325b2fcb37446f, ; 383: lib_System.Private.Xml.dll.so => 135
	i64 u0xc5a0f4b95a699af7, ; 384: lib_System.Private.Uri.dll.so => 133
	i64 u0xc5cdcd5b6277579e, ; 385: lib_System.Security.Cryptography.Algorithms.dll.so => 143
	i64 u0xc7c01e7d7c93a110, ; 386: System.Text.Encoding.Extensions.dll => 146
	i64 u0xc7ce851898a4548e, ; 387: lib_System.Web.HttpUtility.dll.so => 154
	i64 u0xc858a28d9ee5a6c5, ; 388: lib_System.Collections.Specialized.dll.so => 104
	i64 u0xc9e54b32fc19baf3, ; 389: lib_CommunityToolkit.Maui.dll.so => 35
	i64 u0xca3a723e7342c5b6, ; 390: lib-tr-Microsoft.Maui.Controls.resources.dll.so => 28
	i64 u0xcab3493c70141c2d, ; 391: pl/Microsoft.Maui.Controls.resources => 20
	i64 u0xcace8b9ca412599f, ; 392: MuizzaApp1 => 99
	i64 u0xcacfddc9f7c6de76, ; 393: ro/Microsoft.Maui.Controls.resources.dll => 23
	i64 u0xcbd4fdd9cef4a294, ; 394: lib__Microsoft.Android.Resource.Designer.dll.so => 34
	i64 u0xcc2876b32ef2794c, ; 395: lib_System.Text.RegularExpressions.dll.so => 149
	i64 u0xcc5c3bb714c4561e, ; 396: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 97
	i64 u0xcc76886e09b88260, ; 397: Xamarin.KotlinX.Serialization.Core.Jvm.dll => 98
	i64 u0xccf25c4b634ccd3a, ; 398: zh-Hans/Microsoft.Maui.Controls.resources.dll => 32
	i64 u0xccf78fa9cfab41e9, ; 399: lib_MuizzaApp1.dll.so => 99
	i64 u0xcd10a42808629144, ; 400: System.Net.Requests => 128
	i64 u0xcdd0c48b6937b21c, ; 401: Xamarin.AndroidX.SwipeRefreshLayout => 87
	i64 u0xcf23d8093f3ceadf, ; 402: System.Diagnostics.DiagnosticSource.dll => 111
	i64 u0xcf5ff6b6b2c4c382, ; 403: System.Net.Mail.dll => 124
	i64 u0xcf8fc898f98b0d34, ; 404: System.Private.Xml.Linq => 134
	i64 u0xd1194e1d8a8de83c, ; 405: lib_Xamarin.AndroidX.Lifecycle.Common.Jvm.dll.so => 76
	i64 u0xd16fd7fb9bbcd43e, ; 406: Microsoft.Extensions.Diagnostics.Abstractions => 45
	i64 u0xd333d0af9e423810, ; 407: System.Runtime.InteropServices => 137
	i64 u0xd3426d966bb704f5, ; 408: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 67
	i64 u0xd3651b6fc3125825, ; 409: System.Private.Uri.dll => 133
	i64 u0xd373685349b1fe8b, ; 410: Microsoft.Extensions.Logging.dll => 47
	i64 u0xd3e4c8d6a2d5d470, ; 411: it/Microsoft.Maui.Controls.resources => 14
	i64 u0xd4645626dffec99d, ; 412: lib_Microsoft.Extensions.DependencyInjection.Abstractions.dll.so => 43
	i64 u0xd5507e11a2b2839f, ; 413: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 79
	i64 u0xd60815f26a12e140, ; 414: Microsoft.Extensions.Logging.Debug.dll => 49
	i64 u0xd6694f8359737e4e, ; 415: Xamarin.AndroidX.SavedState => 86
	i64 u0xd6d21782156bc35b, ; 416: Xamarin.AndroidX.SwipeRefreshLayout.dll => 87
	i64 u0xd72329819cbbbc44, ; 417: lib_Microsoft.Extensions.Configuration.Abstractions.dll.so => 39
	i64 u0xd7b3764ada9d341d, ; 418: lib_Microsoft.Extensions.Logging.Abstractions.dll.so => 48
	i64 u0xd7f0088bc5ad71f2, ; 419: Xamarin.AndroidX.VersionedParcelable => 88
	i64 u0xda1dfa4c534a9251, ; 420: Microsoft.Extensions.DependencyInjection => 42
	i64 u0xdad05a11827959a3, ; 421: System.Collections.NonGeneric.dll => 103
	i64 u0xdb5383ab5865c007, ; 422: lib-vi-Microsoft.Maui.Controls.resources.dll.so => 30
	i64 u0xdbeda89f832aa805, ; 423: vi/Microsoft.Maui.Controls.resources.dll => 30
	i64 u0xdbf9607a441b4505, ; 424: System.Linq => 120
	i64 u0xdce2c53525640bf3, ; 425: Microsoft.Extensions.Logging => 47
	i64 u0xdd2b722d78ef5f43, ; 426: System.Runtime.dll => 142
	i64 u0xdd67031857c72f96, ; 427: lib_System.Text.Encodings.Web.dll.so => 147
	i64 u0xdde30e6b77aa6f6c, ; 428: lib-zh-Hans-Microsoft.Maui.Controls.resources.dll.so => 32
	i64 u0xde8769ebda7d8647, ; 429: hr/Microsoft.Maui.Controls.resources.dll => 11
	i64 u0xdf3b81f65864db44, ; 430: Microsoft.Extensions.Configuration.UserSecrets.dll => 41
	i64 u0xe0142572c095a480, ; 431: Xamarin.AndroidX.AppCompat.dll => 66
	i64 u0xe02f89350ec78051, ; 432: Xamarin.AndroidX.CoordinatorLayout.dll => 70
	i64 u0xe10b760bb1462e7a, ; 433: lib_System.Security.Cryptography.Primitives.dll.so => 144
	i64 u0xe192a588d4410686, ; 434: lib_System.IO.Pipelines.dll.so => 118
	i64 u0xe1a08bd3fa539e0d, ; 435: System.Runtime.Loader => 138
	i64 u0xe1b52f9f816c70ef, ; 436: System.Private.Xml.Linq.dll => 134
	i64 u0xe1ecfdb7fff86067, ; 437: System.Net.Security.dll => 129
	i64 u0xe2420585aeceb728, ; 438: System.Net.Requests.dll => 128
	i64 u0xe29b73bc11392966, ; 439: lib-id-Microsoft.Maui.Controls.resources.dll.so => 13
	i64 u0xe3811d68d4fe8463, ; 440: pt-BR/Microsoft.Maui.Controls.resources.dll => 21
	i64 u0xe3a586956771a0ed, ; 441: lib_SQLite-net.dll.so => 60
	i64 u0xe4507486c308efd4, ; 442: lib_Xamarin.GooglePlayServices.Location.dll.so => 94
	i64 u0xe494f7ced4ecd10a, ; 443: hu/Microsoft.Maui.Controls.resources.dll => 12
	i64 u0xe4a9b1e40d1e8917, ; 444: lib-fi-Microsoft.Maui.Controls.resources.dll.so => 7
	i64 u0xe4f74a0b5bf9703f, ; 445: System.Runtime.Serialization.Primitives => 141
	i64 u0xe5434e8a119ceb69, ; 446: lib_Mono.Android.dll.so => 162
	i64 u0xe89a2a9ef110899b, ; 447: System.Drawing.dll => 114
	i64 u0xed19c616b3fcb7eb, ; 448: Xamarin.AndroidX.VersionedParcelable.dll => 88
	i64 u0xedc4817167106c23, ; 449: System.Net.Sockets.dll => 130
	i64 u0xedc632067fb20ff3, ; 450: System.Memory.dll => 121
	i64 u0xedc8e4ca71a02a8b, ; 451: Xamarin.AndroidX.Navigation.Runtime.dll => 83
	i64 u0xeeb7ebb80150501b, ; 452: lib_Xamarin.AndroidX.Collection.Jvm.dll.so => 69
	i64 u0xef72742e1bcca27a, ; 453: Microsoft.Maui.Essentials.dll => 55
	i64 u0xefec0b7fdc57ec42, ; 454: Xamarin.AndroidX.Activity => 65
	i64 u0xf00c29406ea45e19, ; 455: es/Microsoft.Maui.Controls.resources.dll => 6
	i64 u0xf09e47b6ae914f6e, ; 456: System.Net.NameResolution => 125
	i64 u0xf11b621fc87b983f, ; 457: Microsoft.Maui.Controls.Xaml.dll => 53
	i64 u0xf1c4b4005493d871, ; 458: System.Formats.Asn1.dll => 115
	i64 u0xf238bd79489d3a96, ; 459: lib-nl-Microsoft.Maui.Controls.resources.dll.so => 19
	i64 u0xf37221fda4ef8830, ; 460: lib_Xamarin.Google.Android.Material.dll.so => 91
	i64 u0xf3ddfe05336abf29, ; 461: System => 157
	i64 u0xf4c1dd70a5496a17, ; 462: System.IO.Compression => 117
	i64 u0xf6077741019d7428, ; 463: Xamarin.AndroidX.CoordinatorLayout => 70
	i64 u0xf6de7fa3776f8927, ; 464: lib_Microsoft.Extensions.Configuration.Json.dll.so => 40
	i64 u0xf77b20923f07c667, ; 465: de/Microsoft.Maui.Controls.resources.dll => 4
	i64 u0xf7e2cac4c45067b3, ; 466: lib_System.Numerics.Vectors.dll.so => 131
	i64 u0xf7e74930e0e3d214, ; 467: zh-HK/Microsoft.Maui.Controls.resources.dll => 31
	i64 u0xf7fa0bf77fe677cc, ; 468: Newtonsoft.Json.dll => 58
	i64 u0xf84773b5c81e3cef, ; 469: lib-uk-Microsoft.Maui.Controls.resources.dll.so => 29
	i64 u0xf8e045dc345b2ea3, ; 470: lib_Xamarin.AndroidX.RecyclerView.dll.so => 85
	i64 u0xf915dc29808193a1, ; 471: System.Web.HttpUtility.dll => 154
	i64 u0xf96c777a2a0686f4, ; 472: hi/Microsoft.Maui.Controls.resources.dll => 10
	i64 u0xf9eec5bb3a6aedc6, ; 473: Microsoft.Extensions.Options => 50
	i64 u0xfa3f278f288b0e84, ; 474: lib_System.Net.Security.dll.so => 129
	i64 u0xfa5ed7226d978949, ; 475: lib-ar-Microsoft.Maui.Controls.resources.dll.so => 0
	i64 u0xfa645d91e9fc4cba, ; 476: System.Threading.Thread => 152
	i64 u0xfb022853d73b7fa5, ; 477: lib_SQLitePCLRaw.batteries_v2.dll.so => 61
	i64 u0xfb3cb754cb2d9fc0, ; 478: lib_Plugin.LocalNotification.dll.so => 59
	i64 u0xfbf0a31c9fc34bc4, ; 479: lib_System.Net.Http.dll.so => 123
	i64 u0xfc6b7527cc280b3f, ; 480: lib_System.Runtime.Serialization.Formatters.dll.so => 140
	i64 u0xfc719aec26adf9d9, ; 481: Xamarin.AndroidX.Navigation.Fragment.dll => 82
	i64 u0xfd22f00870e40ae0, ; 482: lib_Xamarin.AndroidX.DrawerLayout.dll.so => 74
	i64 u0xfd49b3c1a76e2748, ; 483: System.Runtime.InteropServices.RuntimeInformation => 136
	i64 u0xfd536c702f64dc47, ; 484: System.Text.Encoding.Extensions => 146
	i64 u0xfd583f7657b6a1cb, ; 485: Xamarin.AndroidX.Fragment => 75
	i64 u0xfdbe4710aa9beeff, ; 486: CommunityToolkit.Maui => 35
	i64 u0xfeae9952cf03b8cb, ; 487: tr/Microsoft.Maui.Controls.resources => 28
	i64 u0xff9b54613e0d2cc8 ; 488: System.Net.Http.Json => 122
], align 8

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [489 x i32] [
	i32 87, i32 83, i32 36, i32 161, i32 66, i32 64, i32 24, i32 2,
	i32 30, i32 127, i32 95, i32 85, i32 105, i32 54, i32 31, i32 155,
	i32 69, i32 24, i32 103, i32 49, i32 74, i32 50, i32 103, i32 93,
	i32 145, i32 150, i32 25, i32 98, i32 89, i32 21, i32 162, i32 55,
	i32 124, i32 41, i32 125, i32 73, i32 116, i32 144, i32 124, i32 85,
	i32 8, i32 160, i32 9, i32 43, i32 143, i32 158, i32 12, i32 147,
	i32 98, i32 18, i32 101, i32 157, i32 27, i32 44, i32 161, i32 84,
	i32 16, i32 151, i32 50, i32 116, i32 142, i32 27, i32 93, i32 152,
	i32 109, i32 71, i32 141, i32 8, i32 61, i32 96, i32 51, i32 13,
	i32 11, i32 160, i32 127, i32 45, i32 29, i32 126, i32 112, i32 7,
	i32 149, i32 115, i32 33, i32 20, i32 134, i32 153, i32 26, i32 148,
	i32 5, i32 156, i32 46, i32 75, i32 34, i32 68, i32 113, i32 8,
	i32 156, i32 102, i32 6, i32 130, i32 54, i32 2, i32 52, i32 90,
	i32 38, i32 102, i32 73, i32 125, i32 94, i32 89, i32 1, i32 58,
	i32 144, i32 146, i32 96, i32 41, i32 155, i32 71, i32 61, i32 81,
	i32 67, i32 158, i32 162, i32 20, i32 100, i32 141, i32 96, i32 112,
	i32 24, i32 155, i32 22, i32 132, i32 88, i32 84, i32 148, i32 122,
	i32 80, i32 126, i32 119, i32 135, i32 138, i32 14, i32 80, i32 57,
	i32 161, i32 150, i32 1, i32 63, i32 52, i32 37, i32 78, i32 114,
	i32 127, i32 110, i32 71, i32 56, i32 25, i32 126, i32 136, i32 31,
	i32 142, i32 76, i32 104, i32 99, i32 133, i32 62, i32 159, i32 111,
	i32 15, i32 42, i32 100, i32 70, i32 153, i32 108, i32 3, i32 92,
	i32 47, i32 151, i32 137, i32 69, i32 104, i32 147, i32 106, i32 156,
	i32 110, i32 63, i32 5, i32 42, i32 97, i32 121, i32 53, i32 4,
	i32 138, i32 159, i32 102, i32 91, i32 143, i32 35, i32 52, i32 139,
	i32 109, i32 78, i32 72, i32 3, i32 113, i32 115, i32 9, i32 63,
	i32 137, i32 18, i32 57, i32 56, i32 51, i32 72, i32 51, i32 82,
	i32 54, i32 2, i32 28, i32 18, i32 45, i32 14, i32 106, i32 11,
	i32 121, i32 44, i32 59, i32 38, i32 86, i32 139, i32 40, i32 17,
	i32 27, i32 75, i32 94, i32 46, i32 7, i32 107, i32 25, i32 4,
	i32 93, i32 36, i32 17, i32 131, i32 49, i32 105, i32 132, i32 95,
	i32 108, i32 89, i32 39, i32 77, i32 64, i32 157, i32 33, i32 66,
	i32 68, i32 114, i32 29, i32 60, i32 32, i32 151, i32 33, i32 38,
	i32 152, i32 116, i32 55, i32 97, i32 158, i32 106, i32 136, i32 80,
	i32 111, i32 9, i32 72, i32 153, i32 101, i32 58, i32 81, i32 10,
	i32 23, i32 62, i32 95, i32 22, i32 21, i32 62, i32 112, i32 34,
	i32 117, i32 78, i32 53, i32 73, i32 40, i32 148, i32 120, i32 1,
	i32 57, i32 17, i32 117, i32 6, i32 13, i32 56, i32 92, i32 108,
	i32 101, i32 119, i32 37, i32 83, i32 16, i32 65, i32 39, i32 19,
	i32 81, i32 77, i32 59, i32 145, i32 91, i32 84, i32 118, i32 16,
	i32 44, i32 37, i32 110, i32 140, i32 131, i32 145, i32 86, i32 74,
	i32 76, i32 12, i32 92, i32 36, i32 48, i32 135, i32 123, i32 43,
	i32 5, i32 119, i32 139, i32 77, i32 23, i32 150, i32 19, i32 154,
	i32 107, i32 129, i32 160, i32 132, i32 60, i32 79, i32 26, i32 149,
	i32 3, i32 68, i32 10, i32 0, i32 118, i32 48, i32 113, i32 26,
	i32 159, i32 46, i32 100, i32 22, i32 15, i32 105, i32 130, i32 140,
	i32 64, i32 123, i32 90, i32 67, i32 122, i32 0, i32 109, i32 120,
	i32 65, i32 15, i32 90, i32 79, i32 107, i32 128, i32 82, i32 135,
	i32 133, i32 143, i32 146, i32 154, i32 104, i32 35, i32 28, i32 20,
	i32 99, i32 23, i32 34, i32 149, i32 97, i32 98, i32 32, i32 99,
	i32 128, i32 87, i32 111, i32 124, i32 134, i32 76, i32 45, i32 137,
	i32 67, i32 133, i32 47, i32 14, i32 43, i32 79, i32 49, i32 86,
	i32 87, i32 39, i32 48, i32 88, i32 42, i32 103, i32 30, i32 30,
	i32 120, i32 47, i32 142, i32 147, i32 32, i32 11, i32 41, i32 66,
	i32 70, i32 144, i32 118, i32 138, i32 134, i32 129, i32 128, i32 13,
	i32 21, i32 60, i32 94, i32 12, i32 7, i32 141, i32 162, i32 114,
	i32 88, i32 130, i32 121, i32 83, i32 69, i32 55, i32 65, i32 6,
	i32 125, i32 53, i32 115, i32 19, i32 91, i32 157, i32 117, i32 70,
	i32 40, i32 4, i32 131, i32 31, i32 58, i32 29, i32 85, i32 154,
	i32 10, i32 50, i32 129, i32 0, i32 152, i32 61, i32 59, i32 123,
	i32 140, i32 82, i32 74, i32 136, i32 146, i32 75, i32 35, i32 28,
	i32 122
], align 4

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
@.str.0 = private unnamed_addr constant [40 x i8] c"get_function_pointer MUST be specified\0A\00", align 1

;MarshalMethodName
@.MarshalMethodName.0_name = private unnamed_addr constant [1 x i8] c"\00", align 1

; External functions

; Function attributes: noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8"
declare void @abort() local_unnamed_addr #2

; Function attributes: nofree nounwind
declare noundef i32 @puts(ptr noundef) local_unnamed_addr #1
attributes #0 = { memory(write, argmem: none, inaccessiblemem: none) "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+fix-cortex-a53-835769,+neon,+outline-atomics,+v8a" uwtable willreturn }
attributes #1 = { nofree nounwind }
attributes #2 = { noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+fix-cortex-a53-835769,+neon,+outline-atomics,+v8a" }

; Metadata
!llvm.module.flags = !{!0, !1, !7, !8, !9, !10}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!llvm.ident = !{!2}
!2 = !{!".NET for Android remotes/origin/release/9.0.1xx @ 278e101698269c9bc8840aa94d72e7f24066a96d"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C++ TBAA"}
!7 = !{i32 1, !"branch-target-enforcement", i32 0}
!8 = !{i32 1, !"sign-return-address", i32 0}
!9 = !{i32 1, !"sign-return-address-all", i32 0}
!10 = !{i32 1, !"sign-return-address-with-bkey", i32 0}
