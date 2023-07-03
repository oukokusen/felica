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

            '�߂�l
            Dim cardNumber As String = ""

            Dim hCard As IntPtr
            Dim hContext As IntPtr

            ' --- ���\�[�X�}�l�[�W���R���e�L�X�g���m������ ---
            Dim URet As UInteger = SCardEstablishContext(
                SCARD_SCOPE_USER, IntPtr.Zero, IntPtr.Zero, hContext)
            If URet <> SCARD_S_SUCCESS Then
                ErrorRoutine(hCard, hContext)
                cardNumber = "-111"
                GoTo over
            End If

            ' --- �J�[�h���[�_�̃��X�g���擾���� ---
            ' �o�b�t�@�����擾����
            Dim pcchReaders As UInteger ' mszReaders�o�b�t�@��
            URet = SCardListReaders(hContext, Nothing, Nothing, pcchReaders)
            If URet <> SCARD_S_SUCCESS Then
                ErrorRoutine(hCard, hContext)
                cardNumber = "-111"
                GoTo over
            End If
            ' Byte�̔z��Ńo�b�t�@��p�ӂ��A���X�g���擾����
            Dim mszReaders As Byte() =
                New Byte(Convert.ToInt32(pcchReaders) * 2 - 1) {}
            URet = SCardListReaders(hContext, Nothing, mszReaders, pcchReaders)
            If URet <> SCARD_S_SUCCESS Then
                ErrorRoutine(hCard, hContext)
                cardNumber = "-111"
                GoTo over
            End If
            ' Byte�z��𕶎��ɕϊ����ĂP�ڂ̃��[�_�����擾����
            Dim szReader As String =
                System.Text.Encoding.ASCII.GetString(mszReaders).
                Split(vbNullChar.ToCharArray)(0)


            ' --- ���[�_�ɃJ�[�h�����������̂�҂� ---
            Dim dwTimeout As Integer = System.Threading.Timeout.Infinite
            Dim rgReaderStates(0) As SCARD_READERSTATE  ' �Ď��Ώۃ��[�_�p
            rgReaderStates(0).szReader = szReader       ' �Ď��Ώۂ̃��[�_��
            ' �����ɏ���̃��|�[�g����M����ݒ�
            rgReaderStates(0).dwCurrentState = SCARD_STATE_UNAWARE
            ' ���[�_�ɃJ�[�h�����������܂őҋ@
            Do
                ' �Ď�����
                URet = SCardGetStatusChange(hContext, dwTimeout,
                    rgReaderStates(0), rgReaderStates.Count)
                If URet <> SCARD_S_SUCCESS Then
                    ErrorRoutine(hCard, hContext)
                    cardNumber = "-115"
                    GoTo over
                End If
                ' �J�[�h���m�F���ꂽ�烋�[�v�𔲂���
                If (rgReaderStates(0).dwEventState And
                    SCARD_STATE_PRESENT) <> 0 Then
                    Exit Do
                Else
                    Thread.Sleep(intPolling)
                End If

            Loop

            ' --- �A�v���P�[�V�����ƃJ�[�h�̐ڑ����m������ ---
            Dim pdwActiveProtocol As IntPtr = IntPtr.Zero
            URet = SCardConnect(hContext, szReader,
                SCARD_SHARE_SHARED, SCARD_PROTOCOL_T0 Or SCARD_PROTOCOL_T1,
                hCard, pdwActiveProtocol)
            If URet <> SCARD_S_SUCCESS Then
                ErrorRoutine(hCard, hContext)
                cardNumber = "999"
                GoTo over
            End If

            ' --- �T�[�r�X���N�G�X�g�𑗐M���f�[�^�o�b�N����M���� ---
            ' ���߃v���g�R���w�b�_�ւ̃|�C���^���쐬����
            Dim pioSendPci As IntPtr    ' �w�b�_�ւ̃|�C���^
            ' winscard.dll�̃n���h�����擾����
            Dim hModule As IntPtr = LoadLibrary("winscard.dll")
            ' �v���g�R���ɉ����ĕ���
            If pdwActiveProtocol = CType(SCARD_PROTOCOL_T0, IntPtr) Then
                ' ����������d�L�����N�^�`���v���g�R���̏ꍇ
                pioSendPci = GetProcAddress(hModule, "g_rgSCardT0Pci")
            ElseIf pdwActiveProtocol = CType(SCARD_PROTOCOL_T1, IntPtr) Then
                ' ����������d�u���b�N�`���v���g�R���̏ꍇ
                pioSendPci = GetProcAddress(hModule, "g_rgSCardT1Pci")
            End If
            ' DLL���W���[�����������
            FreeLibrary(hModule)


            ' �v���e�N�g������
            Dim pbSendBuffer As Byte() = New Byte() {&HFF, &H82, &H0, &H0, &H6, &HA0, &HA1, &HA2, &HA3, &HA4, &HA5, &HA6}
            ' ��M�v���g�R���w�b�_�̃|�C���^
            Dim pioRecvRequest As SCARD_IO_REQUEST = Nothing
            ' �ԐM�f�[�^�p�o�b�t�@
            Dim pbrecvBuffer As Byte() = New Byte(BUFSIZ) {}
            ' �ԐM�f�[�^�p�o�b�t�@�T�C�Y
            Dim pcbRecvLength As Integer = pbrecvBuffer.Length
            ' �T�[�r�X���N�G�X�g�𑗐M���f�[�^�o�b�N����M����
            URet = SCardTransmit(
                hCard, pioSendPci, pbSendBuffer, pbSendBuffer.Length,
                pioRecvRequest, pbrecvBuffer, pcbRecvLength)
            If URet <> SCARD_S_SUCCESS Then
                ErrorRoutine(hCard, hContext)
                cardNumber = "999"
                GoTo over
            End If

            ' �F�؂����{
            pbSendBuffer = New Byte() {&HFF, &H86, &H0, &H0, &H5, &H1, &H0, &H8, &H60, &H0}
            ' ��M�v���g�R���w�b�_�̃|�C���^
            pioRecvRequest = Nothing
            ' �ԐM�f�[�^�p�o�b�t�@
            pbrecvBuffer = New Byte(BUFSIZ) {}
            ' �ԐM�f�[�^�p�o�b�t�@�T�C�Y
            pcbRecvLength = pbrecvBuffer.Length
            ' �T�[�r�X���N�G�X�g�𑗐M���f�[�^�o�b�N����M����
            URet = SCardTransmit(
                hCard, pioSendPci, pbSendBuffer, pbSendBuffer.Length,
                pioRecvRequest, pbrecvBuffer, pcbRecvLength)
            If URet <> SCARD_S_SUCCESS Then
                ErrorRoutine(hCard, hContext)
                cardNumber = "999"
                GoTo over
            End If

            ' �u���b�N�f�[�^��ǂݏo��
            pbSendBuffer = New Byte() {&HFF, &HB0, &H0, &H8, &H10}
            ' ��M�v���g�R���w�b�_�̃|�C���^
            pioRecvRequest = Nothing
            ' �ԐM�f�[�^�p�o�b�t�@
            pbrecvBuffer = New Byte(BUFSIZ) {}
            ' �ԐM�f�[�^�p�o�b�t�@�T�C�Y
            pcbRecvLength = pbrecvBuffer.Length
            ' �T�[�r�X���N�G�X�g�𑗐M���f�[�^�o�b�N����M����
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
        ''' (summary) JSP�̏����ɍ��킹�Ė߂�l��ݒ肷��      (/summary)
        ''' --------------------------------------------------------------------
        Public Function exitForm() As String
            Return 0
        End Function
    End Class

End Namespace
