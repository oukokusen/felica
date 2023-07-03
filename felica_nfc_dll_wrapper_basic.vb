'
' felica_nfc_dll_wrapper_basic.vb
' Copyright 2009 Sony Corporation
'
Imports System
Imports System.Text
Imports System.Runtime.InteropServices
Module FeliCa_Nfc_Dll_Wrapper_Basic
    ' ───────────────────────────────────
    '  定数の宣言      
    ' ───────────────────────────────────
    ''' --------------------------------------------------------------------
    ''' (summary) 定数                                            (/summary)
    ''' --------------------------------------------------------------------
    Public Const SCARD_S_SUCCESS As Integer = 0
    Public Const SCARD_F_INTERNAL_ERROR As Integer = &H80100001
    Public Const SCARD_E_CANCELLED As Integer = &H80100002
    Public Const SCARD_E_INVALID_HANDLE As Integer = &H80100003
    Public Const SCARD_E_INVALID_PARAMETER As Integer = &H80100004
    Public Const SCARD_E_INVALID_TARGET As Integer = &H80100005
    Public Const SCARD_E_NO_MEMORY As Integer = &H80100006
    Public Const SCARD_F_WAITED_TOO_LONG As Integer = &H80100007
    Public Const SCARD_E_INSUFFICIENT_BUFFER As Integer = &H80100008
    Public Const SCARD_E_UNKNOWN_READER As Integer = &H80100009
    Public Const SCARD_E_TIMEOUT As Integer = &H8010000A
    Public Const SCARD_E_SHARING_VIOLATION As Integer = &H8010000B
    Public Const SCARD_E_NO_SMARTCARD As Integer = &H8010000C
    Public Const SCARD_E_UNKNOWN_CARD As Integer = &H8010000D
    Public Const SCARD_E_CANT_DISPOSE As Integer = &H8010000E
    Public Const SCARD_E_PROTO_MISMATCH As Integer = &H8010000F
    Public Const SCARD_E_NOT_READY As Integer = &H80100010
    Public Const SCARD_E_INVALID_VALUE As Integer = &H80100011
    Public Const SCARD_E_SYSTEM_CANCELLED As Integer = &H80100012
    Public Const SCARD_E_COMM_ERROR As Integer = &H80100013
    Public Const SCARD_F_UNKNOWN_ERROR As Integer = &H80100014
    Public Const SCARD_E_INVALID_ATR As Integer = &H80100015
    Public Const SCARD_E_NOT_TRANSACTED As Integer = &H80100016
    Public Const SCARD_E_READER_UNAVAILABLE As Integer = &H80100017
    Public Const SCARD_P_SHUTDOWN As Integer = &H80100018
    Public Const SCARD_E_PCI_TOO_SMALL As Integer = &H80100019
    Public Const SCARD_E_READER_UNSUPPORTED As Integer = &H8010001A
    Public Const SCARD_E_DUPLICATE_READER As Integer = &H8010001B
    Public Const SCARD_E_CARD_UNSUPPORTED As Integer = &H8010001C
    Public Const SCARD_E_NO_SERVICE As Integer = &H8010001D
    Public Const SCARD_E_SERVICE_STOPPED As Integer = &H8010001E
    Public Const SCARD_E_UNEXPECTED As Integer = &H8010001F
    Public Const SCARD_E_ICC_INSTALLATION As Integer = &H80100020
    Public Const SCARD_E_ICC_CREATEORDER As Integer = &H80100021
    Public Const SCARD_E_UNSUPPORTED_FEATURE As Integer = &H80100022
    Public Const SCARD_E_DIR_NOT_FOUND As Integer = &H80100023
    Public Const SCARD_E_FILE_NOT_FOUND As Integer = &H80100024
    Public Const SCARD_E_NO_DIR As Integer = &H80100025
    Public Const SCARD_E_NO_FILE As Integer = &H80100026
    Public Const SCARD_E_NO_ACCESS As Integer = &H80100027
    Public Const SCARD_E_WRITE_TOO_MANY As Integer = &H80100028
    Public Const SCARD_E_BAD_SEEK As Integer = &H80100029
    Public Const SCARD_E_INVALID_CHV As Integer = &H8010002A
    Public Const SCARD_E_UNKNOWN_RES_MNG As Integer = &H8010002B
    Public Const SCARD_E_NO_SUCH_CERTIFICATE As Integer = &H8010002C
    Public Const SCARD_E_CERTIFICATE_UNAVAILABLE As Integer = &H8010002D
    Public Const SCARD_E_NO_READERS_AVAILABLE As Integer = &H8010002E
    Public Const SCARD_E_COMM_DATA_LOST As Integer = &H8010002F
    Public Const SCARD_E_NO_KEY_CONTAINER As Integer = &H80100030
    Public Const SCARD_E_SERVER_TOO_BUSY As Integer = &H80100031
    Public Const SCARD_E_PIN_CACHE_EXPIRED As Integer = &H80100032
    Public Const SCARD_E_NO_PIN_CACHE As Integer = &H80100033
    Public Const SCARD_E_READ_ONLY_CARD As Integer = &H80100034
    Public Const SCARD_W_UNSUPPORTED_CARD As Integer = &H80100065
    Public Const SCARD_W_UNRESPONSIVE_CARD As Integer = &H80100066
    Public Const SCARD_W_UNPOWERED_CARD As Integer = &H80100067
    Public Const SCARD_W_RESET_CARD As Integer = &H80100068
    Public Const SCARD_W_REMOVED_CARD As Integer = &H80100069
    Public Const SCARD_W_SECURITY_VIOLATION As Integer = &H8010006A
    Public Const SCARD_W_WRONG_CHV As Integer = &H8010006B
    Public Const SCARD_W_CHV_BLOCKED As Integer = &H8010006C
    Public Const SCARD_W_EOF As Integer = &H8010006D
    Public Const SCARD_W_CANCELLED_BY_USER As Integer = &H8010006E
    Public Const SCARD_W_CARD_NOT_AUTHENTICATED As Integer = &H8010006F
    Public Const SCARD_W_CACHE_ITEM_NOT_FOUND As Integer = &H80100070
    Public Const SCARD_W_CACHE_ITEM_STALE As Integer = &H80100071
    Public Const SCARD_W_CACHE_ITEM_TOO_BIG As Integer = &H80100072

    Public Const SCARD_PROTOCOL_T0 As Integer = 1
    Public Const SCARD_PROTOCOL_T1 As Integer = 2
    Public Const SCARD_PROTOCOL_RAW As Integer = 4
    Public Const SCARD_SCOPE_USER As UInteger = 0
    Public Const SCARD_SCOPE_TERMINAL As UInteger = 1
    Public Const SCARD_SCOPE_SYSTEM As UInteger = 2
    Public Const SCARD_STATE_UNAWARE As Integer = &H0
    Public Const SCARD_STATE_IGNORE As Integer = &H1
    Public Const SCARD_STATE_CHANGED As Integer = &H2
    Public Const SCARD_STATE_UNKNOWN As Integer = &H4
    Public Const SCARD_STATE_UNAVAILABLE As Integer = &H8
    Public Const SCARD_STATE_EMPTY As Integer = &H10
    Public Const SCARD_STATE_PRESENT As Integer = &H20
    Public Const SCARD_STATE_ATRMATCH As Integer = &H40
    Public Const SCARD_STATE_EXCLUSIVE As Integer = &H80
    Public Const SCARD_STATE_INUSE As Integer = &H100
    Public Const SCARD_STATE_MUTE As Integer = &H200
    Public Const SCARD_STATE_UNPOWERED As Integer = &H400
    Public Const SCARD_SHARE_EXCLUSIVE As Integer = &H1
    Public Const SCARD_SHARE_SHARED As Integer = &H2
    Public Const SCARD_SHARE_DIRECT As Integer = &H3
    Public Const SCARD_LEAVE_CARD As Integer = 0
    Public Const SCARD_RESET_CARD As Integer = 1
    Public Const SCARD_UNPOWER_CARD As Integer = 2
    Public Const SCARD_EJECT_CARD As Integer = 3

    ' ───────────────────────────────────
    '  構造体の宣言      
    ' ───────────────────────────────────
    ''' --------------------------------------------------------------------
    ''' (summary) スマートカードトラッキング用構造体              (/summary)
    ''' --------------------------------------------------------------------
    Public Structure SCARD_READERSTATE
        Public szReader As String           ' モニタしているリーダへのポインタ
        Public pvUserData As IntPtr         ' 不使用
        Public dwCurrentState As UInt32     ' アプリケーションから見た状態
        Public dwEventState As UInt32       ' リソースマネージャから見た状態
        Public cbAtr As UInt32              ' ATRのバイト数
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=36)>' 配列サイズの固定
        Public rgbAtr() As Byte             ' カードのATR
    End Structure

    ' ───────────────────────────────────
    '  クラスの宣言      
    ' ───────────────────────────────────
    ''' --------------------------------------------------------------------
    ''' (summary) プロトコル制御情報クラス                        (/summary)
    ''' --------------------------------------------------------------------
    Public Class SCARD_IO_REQUEST
        Friend dwProtocol As UInteger   ' 使用中のプロトコル
        Friend cbPciLength As Integer   ' クラスのサイズとPCI固有の情報
    End Class

    ' ───────────────────────────────────
    '  変数の宣言      
    ' ───────────────────────────────────
    ' ───────────────────────────────────
    '  API関数の宣言      
    ' ───────────────────────────────────
    ''' --------------------------------------------------------------------
    ''' (summary) リソースマネージャコンテキストを確立する        (/summary)
    ''' (param name="dwScope")     コンテキストのスコープ           (/param)
    ''' (param name="pvReserved1") 予約変数。NULLにする             (/param)
    ''' (param name="pvReserved2") 予約変数。NULLにする             (/param)
    ''' (param name="phContext")   コンテキストへのハンドル         (/param)
    ''' (returns) エラーコード                                    (/returns)
    ''' --------------------------------------------------------------------
    <DllImport("winscard.dll")>
    Public Function SCardEstablishContext(
            ByVal dwScope As UInteger,
            ByVal pvReserved1 As IntPtr,
            ByVal pvReserved2 As IntPtr,
            ByRef phContext As IntPtr) _
            As UInteger
    End Function

    ''' --------------------------------------------------------------------
    ''' (summary) リーダーグループ中のリーダリストを提供する      (/summary)
    ''' (param name="hContext")    コンテキストを識別するハンドル   (/param)
    ''' (param name="mszGroups")   リーダーグループの名前           (/param)
    ''' (param name="mszReaders")  カードリーダー一覧               (/param)
    ''' (param name="pcchReaders") mszReadersバッファ長             (/param)
    ''' (returns) エラーコード                                    (/returns)
    ''' --------------------------------------------------------------------
    <DllImport("winscard.dll")>
    Public Function SCardListReaders(
            ByVal hContext As IntPtr,
            ByVal mszGroups As Byte(),
            ByVal mszReaders As Byte(),
            ByRef pcchReaders As UInt32) _
            As UInteger
    End Function

    ''' --------------------------------------------------------------------
    ''' (summary) リーダが特定の状態になるまで実行を待機する      (/summary)
    ''' (param name="hContext")       コンテキストを識別するハンドル(/param)
    ''' (param name="dwTimeout")      待ち時間[ms]。INFINITEなら∞  (/param)
    ''' (param name="rgReaderStates") 監視するリーダ構造体の配列    (/param)
    ''' (param name="cReaders")       rgReaderStatesの配列サイズ    (/param)
    ''' (returns) エラーコード                                    (/returns)
    ''' --------------------------------------------------------------------
    <DllImport("winscard.dll")>
    Public Function SCardGetStatusChange(
            ByVal hContext As IntPtr,
            ByVal dwTimeout As Integer,
            ByRef rgReaderStates As SCARD_READERSTATE,
            ByVal cReaders As Integer) _
            As UInteger
    End Function

    ''' --------------------------------------------------------------------
    ''' (summary> アプリケーションとカードの接続を確立する        (/summary)
    ''' (param name="hContext")             コンテキスト識別ハンドル(/param)
    ''' (param name="szReader")             リーダ名                (/param)
    ''' (param name="dwShareMode")          他アプリ排除フラグ      (/param)
    ''' (param name="dwPreferredProtocols") 接続プロトコル          (/param)
    ''' (param name="phCard")               カードへのハンドル      (/param)
    ''' (param name="pdwActiveProtocol")    確立されたプロトコル    (/param)
    ''' (returns) エラーコード                                    (/returns)
    ''' --------------------------------------------------------------------
    <DllImport("winscard.dll")>
    Public Function SCardConnect(
            ByVal hContext As IntPtr,
            ByVal szReader As String,
            ByVal dwShareMode As UInteger,
            ByVal dwPreferredProtocols As UInteger,
            ByRef phCard As IntPtr,
            ByRef pdwActiveProtocol As IntPtr) _
            As UInteger
    End Function

    ''' --------------------------------------------------------------------
    ''' (summary) リーダの直接制御                                (/summary)
    ''' (param name="hCard">SCardConnectから返される参照値          (/param)
    ''' (param name="dwControlCode)    操作の制御コード             (/param)
    ''' (param name="lpInBuffer")      データバッファへのポインタ   (/param)
    ''' (param name="cbInBufferSize")  lpInBufferのサイズ           (/param)
    ''' (param name="lpOutBuffer")     出力バッファへのポインタ     (/param)
    ''' (param name="cbOutBufferSize") lpOutBufferのサイズ          (/param)
    ''' (param name="lpBytesReturned") lpOutBufferに格納されたサイズ(/param)
    ''' (returns> エラーコード                                    (/returns)
    ''' --------------------------------------------------------------------
    <DllImport("winscard.dll")>
    Public Function SCardControl(
            ByVal hCard As IntPtr,
            ByVal dwControlCode As Integer,
            ByVal lpInBuffer As Byte(),
            ByVal cbInBufferSize As Integer,
            ByVal lpOutBuffer As Byte(),
            ByVal cbOutBufferSize As Integer,
            ByRef lpBytesReturned As Integer) _
            As UInteger
    End Function

    ''' --------------------------------------------------------------------
    ''' (summary> カードの状態                                    (/summary)
    ''' (param name="hCard")          SCardConnectから返される参照値(/param)
    ''' (param name="mszReaderNames") リーダから返された表示名      (/param)
    ''' (param name="pcchReaderLen")  szReaderName bufferのサイズ   (/param)
    ''' (param name="pdwState")       リーダ内のカードの状態        (/param)
    ''' (param name="pdwProtocol")    現在プロトコル                (/param)
    ''' (param name="pbAtr")          ATR文字列へのポインタ         (/param)
    ''' (param name="pcbAtrLen")      用意したサイズと受信したサイズ(/param)
    ''' (returns) エラーコード                                    (/returns)
    ''' --------------------------------------------------------------------
    <DllImport("winscard.dll")>
    Public Function SCardStatus(
            ByVal hCard As IntPtr,
            ByVal mszReaderNames As String,
            ByVal pcchReaderLen As Integer,
            ByVal pdwState As Integer,
            ByVal pdwProtocol As Integer,
            ByVal pbAtr As Byte,
            ByVal pcbAtrLen As Integer) _
            As UInteger
    End Function

    ''' --------------------------------------------------------------------
    ''' (summary> サービスリクエストを送信しデータバックを受信する(/summary)
    ''' (param name="hCard")          SCardConnectから返される参照値(/param)
    ''' (param name="pioSendPci)     命令プロトコルヘッダのポインタ(/param)
    ''' (param name="pbSendBuffer")   送信データへのポインタ        (/param)
    ''' (param name="cbSendLength")   pbSendBufferパラメーターの長さ(/param)
    ''' (param name="pioRecvPci")     受信プロトコルヘッダのポインタ(/param)
    ''' (param name="pbRecvBuffer")   返信データへのポインタ        (/param)
    ''' (param name="pcbRecvvLength") pbRecvBufferパラメーターの長さ(/param)
    ''' (returns) エラーコード                                    (/returns)
    ''' --------------------------------------------------------------------
    <DllImport("winscard.dll")>
    Public Function SCardTransmit(
            ByVal hCard As IntPtr,
            ByVal pioSendPci As IntPtr,
            ByVal pbSendBuffer As Byte(),
            ByVal cbSendLength As Integer,
            ByVal pioRecvPci As SCARD_IO_REQUEST,
            ByVal pbRecvBuffer As Byte(),
            ByRef pcbRecvvLength As Integer) _
            As UInteger
    End Function

    ''' --------------------------------------------------------------------
    ''' (summary) 接続を終了する                                  (/summary)
    ''' (param name="hCard")         SCardConnectから返される参照値 (/param)
    ''' (param name="dwDisposition)  実行時のカードへのアクション   (/param)
    ''' (returns) エラーコード                                    (/returns)
    ''' --------------------------------------------------------------------
    <DllImport("winscard.dll")>
    Public Function SCardDisconnect(
            ByVal hCard As IntPtr,
            ByVal dwDisposition As Integer) _
            As UInteger
    End Function

    ''' --------------------------------------------------------------------
    ''' (summary)コンテクストを閉じ、リソースを解放する           (/summary)
    ''' (param name="hContext") コンテキスト識別ハンドル            (/param)
    ''' (returns) エラーコード                                    (/returns)
    ''' --------------------------------------------------------------------
    <DllImport("winscard.dll")>
    Public Function SCardReleaseContext(
            ByVal hContext As IntPtr) _
            As UInteger
    End Function

    ' ───────────────────────────────────

    ''' --------------------------------------------------------------------
    ''' (summary) DLLモジュールを解放する                         (/summary)
    ''' (param name="handle") ロードされたライブラリへのハンドル    (/param)
    ''' --------------------------------------------------------------------
    <DllImport("kernel32.dll")>
    Public Sub FreeLibrary(ByVal handle As IntPtr)
    End Sub

    ''' --------------------------------------------------------------------
    ''' (summary) DLLからの関数や変数へのアドレスを取得する       (/summary)
    ''' (param name="hModule)    DLLモジュールのハンドル           (/param)
    ''' (param name="lpProcName") 関数または変数名、または関数の序数(/param)
    ''' (returns) 関数または変数のアドレス                        (/returns)
    ''' --------------------------------------------------------------------
    <DllImport("kernel32.dll")>
    Public Function GetProcAddress(
            ByVal hModule As IntPtr,
            ByVal lpProcName As String) _
            As IntPtr
    End Function

    ''' --------------------------------------------------------------------
    ''' (summary) 指定したモジュールをアドレス空間にロードする    (/summary)
    ''' (param name="lpLibFileName"> モジュール名                   (/param)
    ''' (returns) 成功したらモジュールのハンドル、失敗したらNULL  (/returns)
    ''' --------------------------------------------------------------------
    <DllImport("kernel32.dll")>
    Public Function LoadLibrary(
            ByVal lpLibFileName As String) _
            As IntPtr
    End Function
End Module