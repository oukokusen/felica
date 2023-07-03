'
' nfc_sample_01.vb
' Copyright 2009 Sony Corporation
'
Imports System
Imports System.Reflection
Imports System.Text
Imports System.Threading
Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Windows.Forms
Imports System.Linq

Namespace CardSecure

    ' "CardSecure.Felica"  
    Public Class Felica

        Private Const BUFSIZ As Integer = 511
        Private ReadOnly intPolling As Integer = 1000

        Public Function getCardCode(ByVal cardNo As String) As String

            '戻り値
            Dim cardNumber As String = ""

            Dim hCard As IntPtr
            Dim hContext As IntPtr

            ' --- リソースマネージャコンテキストを確立する ---
            Dim URet As UInteger = SCardEstablishContext(
                SCARD_SCOPE_USER, IntPtr.Zero, IntPtr.Zero, hContext)
            If URet <> SCARD_S_SUCCESS Then
                ErrorRoutine(hCard, hContext)
                cardNumber = "-111"
                GoTo over
            End If

            ' --- カードリーダのリストを取得する ---
            ' バッファ長を取得する
            Dim pcchReaders As UInteger ' mszReadersバッファ長
            URet = SCardListReaders(hContext, Nothing, Nothing, pcchReaders)
            If URet <> SCARD_S_SUCCESS Then
                ErrorRoutine(hCard, hContext)
                cardNumber = "-111"
                GoTo over
            End If
            ' Byteの配列でバッファを用意し、リストを取得する
            Dim mszReaders As Byte() =
                New Byte(Convert.ToInt32(pcchReaders) * 2 - 1) {}
            URet = SCardListReaders(hContext, Nothing, mszReaders, pcchReaders)
            If URet <> SCARD_S_SUCCESS Then
                ErrorRoutine(hCard, hContext)
                cardNumber = "-111"
                GoTo over
            End If
            ' Byte配列を文字に変換して１つ目のリーダ名を取得する
            Dim szReader As String =
                System.Text.Encoding.ASCII.GetString(mszReaders).
                Split(vbNullChar.ToCharArray)(0)


            ' --- リーダにカードがかざされるのを待つ ---
            Dim dwTimeout As Integer = System.Threading.Timeout.Infinite
            Dim rgReaderStates(0) As SCARD_READERSTATE  ' 監視対象リーダ用
            rgReaderStates(0).szReader = szReader       ' 監視対象のリーダ名
            ' すぐに初回のレポートを受信する設定
            rgReaderStates(0).dwCurrentState = SCARD_STATE_UNAWARE
            ' リーダにカードがかざされるまで待機
            Do
                ' 監視する
                URet = SCardGetStatusChange(hContext, dwTimeout,
                    rgReaderStates(0), rgReaderStates.Count)
                If URet <> SCARD_S_SUCCESS Then
                    ErrorRoutine(hCard, hContext)
                    cardNumber = "-115"
                    GoTo over
                End If
                ' カードが確認されたらループを抜ける
                If (rgReaderStates(0).dwEventState And
                    SCARD_STATE_PRESENT) <> 0 Then
                    Exit Do
                Else
                    Thread.Sleep(intPolling)
                End If

            Loop

            ' --- アプリケーションとカードの接続を確立する ---
            Dim pdwActiveProtocol As IntPtr = IntPtr.Zero
            URet = SCardConnect(hContext, szReader,
                SCARD_SHARE_SHARED, SCARD_PROTOCOL_T0 Or SCARD_PROTOCOL_T1,
                hCard, pdwActiveProtocol)
            If URet <> SCARD_S_SUCCESS Then
                ErrorRoutine(hCard, hContext)
                cardNumber = "999"
                GoTo over
            End If

            ' --- サービスリクエストを送信しデータバックを受信する ---
            ' 命令プロトコルヘッダへのポインタを作成する
            Dim pioSendPci As IntPtr    ' ヘッダへのポインタ
            ' winscard.dllのハンドラを取得する
            Dim hModule As IntPtr = LoadLibrary("winscard.dll")
            ' プロトコルに応じて分岐
            If pdwActiveProtocol = CType(SCARD_PROTOCOL_T0, IntPtr) Then
                ' 調歩式半二重キャラクタ伝送プロトコルの場合
                pioSendPci = GetProcAddress(hModule, "g_rgSCardT0Pci")
            ElseIf pdwActiveProtocol = CType(SCARD_PROTOCOL_T1, IntPtr) Then
                ' 調歩式半二重ブロック伝送プロトコルの場合
                pioSendPci = GetProcAddress(hModule, "g_rgSCardT1Pci")
            End If
            ' DLLモジュールを解放する
            FreeLibrary(hModule)


            ' プロテクトを解除
            Dim pbSendBuffer As Byte() = New Byte() {&HFF, &H82, &H0, &H0, &H6, &HA0, &HA1, &HA2, &HA3, &HA4, &HA5, &HA6}
            ' 受信プロトコルヘッダのポインタ
            Dim pioRecvRequest As SCARD_IO_REQUEST = Nothing
            ' 返信データ用バッファ
            Dim pbrecvBuffer As Byte() = New Byte(BUFSIZ) {}
            ' 返信データ用バッファサイズ
            Dim pcbRecvLength As Integer = pbrecvBuffer.Length
            ' サービスリクエストを送信しデータバックを受信する
            URet = SCardTransmit(
                hCard, pioSendPci, pbSendBuffer, pbSendBuffer.Length,
                pioRecvRequest, pbrecvBuffer, pcbRecvLength)
            If URet <> SCARD_S_SUCCESS Then
                ErrorRoutine(hCard, hContext)
                cardNumber = "999"
                GoTo over
            End If

            ' 認証を実施
            pbSendBuffer = New Byte() {&HFF, &H86, &H0, &H0, &H5, &H1, &H0, &H8, &H60, &H0}
            ' 受信プロトコルヘッダのポインタ
            pioRecvRequest = Nothing
            ' 返信データ用バッファ
            pbrecvBuffer = New Byte(BUFSIZ) {}
            ' 返信データ用バッファサイズ
            pcbRecvLength = pbrecvBuffer.Length
            ' サービスリクエストを送信しデータバックを受信する
            URet = SCardTransmit(
                hCard, pioSendPci, pbSendBuffer, pbSendBuffer.Length,
                pioRecvRequest, pbrecvBuffer, pcbRecvLength)
            If URet <> SCARD_S_SUCCESS Then
                ErrorRoutine(hCard, hContext)
                cardNumber = "999"
                GoTo over
            End If

            ' ブロックデータを読み出す
            pbSendBuffer = New Byte() {&HFF, &HB0, &H0, &H8, &H10}
            ' 受信プロトコルヘッダのポインタ
            pioRecvRequest = Nothing
            ' 返信データ用バッファ
            pbrecvBuffer = New Byte(BUFSIZ) {}
            ' 返信データ用バッファサイズ
            pcbRecvLength = pbrecvBuffer.Length
            ' サービスリクエストを送信しデータバックを受信する
            URet = SCardTransmit(
                hCard, pioSendPci, pbSendBuffer, pbSendBuffer.Length,
                pioRecvRequest, pbrecvBuffer, pcbRecvLength)
            If URet <> SCARD_S_SUCCESS Then
                ErrorRoutine(hCard, hContext)
                cardNumber = "999"
                GoTo over
            End If
            Dim i As Integer
            For i = 0 To 6
                cardNumber = cardNumber & Convert.ToChar(pbrecvBuffer(i))
                Console.WriteLine(pbrecvBuffer(i))
            Next

            On Error GoTo over
            ErrorRoutine(hCard, hContext)


over:
            cardNo = cardNumber
            Return cardNumber
        End Function

        Private Sub ErrorRoutine(hCard As IntPtr, hContext As IntPtr)
            SCardDisconnect(hCard, SCARD_LEAVE_CARD)
            SCardReleaseContext(hContext)
            Return
        End Sub
        ''' --------------------------------------------------------------------
        ''' (summary) JSPの処理に合わせて戻り値を設定する      (/summary)
        ''' --------------------------------------------------------------------
        Public Function exitForm() As String
            Return 0
        End Function
    End Class

End Namespace
