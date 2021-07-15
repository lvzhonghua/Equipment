$include (c8051f350.inc)
PS1	DATA	51H		;ѹ��������1����ֵ
PS2	DATA	59H		;ѹ��������2����ֵ
CG1	DATA	70H		;ͨ��1����У��ϵ��
CO1	DATA	73H		;ͨ��1ƫ��У��ϵ��
CG2	DATA	76H		;ͨ��2����У��ϵ��
CO2	DATA	79H		;ͨ��2ƫ��У��ϵ��
EV1	BIT	P0.0		;��ŷ�1
EV2	BIT	P0.1		;��ŷ�2
EV3	BIT	P0.2		;��ŷ�3
ACP	BIT	P0.3		;��ѹ������λ
RUN	BIT	P0.6		;����/ֹͣ����
;---------��ʼ������
	CSEG	AT	0000H
 	LJMP	80H
	CSEG	AT	0003H
	LJMP      INTE0		;�ⲿ�ж�0
	CSEG	AT	000BH
	RETI			;��ʱ��0�ж�
	CSEG	AT	0023H
	LJMP	INTCOM		;�����ж�		
	CSEG	AT	002BH	
	LJMP	INTT2		;��ʱ��2�ж�
	CSEG	AT	003BH
	RETI			;SMBus�ж�
	CSEG	AT	0053H
	LJMP	INTADC		;ADC0�ж�
	CSEG	AT	0073H
	LJMP	INTT3		;TR3�ж�
	DW	0,0,0,0,0

	CSEG	AT	0080H  
	MOV	SP, #0BFH		;ջ��
	CLR	RS0		;�Ĵ�������0
	CLR	RS1
	CLR	EA		;CPU�������е��ж�����
;---------��ʱ��T0��52.8��sʱ�ӷ��������ڶ�����ʱ�ӷ�����19200bps����ʼ��
	MOV	TMOD, #22H	;��ʱ��T0��T1-�Զ���װ�ص�8λ��ʱ��
	MOV	CKCON, #0AH	;��ʱ��T0-ϵͳʱ�ӵ�48��Ƶ��T1-ϵͳʱ��
	MOV	TH0, #0E6H	;��ʱʱ�䣺52.8��s
	MOV	TL0, #0E6H		
;---------��ʱ��T1��UART�����ʷ���������ʼ��
	MOV	TH1, #096H	;�Զ���װֵ
	MOV	TL1, #096H	;UART������Լ115200bps
 	SETB	TR1		;������ʱ�� 
;---------��ʱ��T2����������������ʱ������ʼ��
	MOV	TMR2RLH, #038H	;�Զ���װֵ���ж϶�ʱԼ25mS
	MOV	TMR2RLL, #09EH
	MOV	TMR2H, #038H
	MOV	TMR2L, #09EH
	MOV	60H, #10		;ѹ����������·��ⶨʱ��10S
	MOV	61H, #40		;������ʱ��1S
	CLR	6DH		;ѹ��������1
;---------��ʱ��T3��������ʱ������ʼ��
	MOV	TMR3RLH, #038H	;�Զ���װֵ���ж϶�ʱԼ25mS
	MOV	TMR3RLL, #09EH
	MOV	TMR3H, #038H
	MOV	TMR3L, #09EH
	MOV	TMR3CN, #00H	;16λ�Զ���װ���жϱ�־���㣬�ر�T3
	MOV	62H, #8
;---------IO�ڳ�ʼ��
;	P0.0  -  EV1,		Push-Pull,  Digital
;	P0.1  -  EV2,		Push-Pull,  Digital
;	P0.2  -  EV3,		Push-Pull,  Digital
;	P0.3  -  ACP,		Push-Pull,  Digital
;	P0.4  -  TX0 (UART0),	Open-Drain, Digital
;	P0.5  -  RX0 (UART0),	Open-Drain, Digital
;	P0.6  -  RUN,		Push-Pull,  Digital
;	P0.7  -  NC,		Push-Pull,  Digital
;
;	P1.0  -  NC,		Push-Pull,  Digital
;	P1.1  -  NC,		Push-Pull,  Digital
;	P1.2  -  NC,		Push-Pull,  Digital
;	P1.3  -  NC,		Push-Pull,  Digital
;	P1.4  -  DI,		Open-Drain, Digital
;	P1.5  -  DE,		Open-Drain, Digital
;	P1.6  -  RO,		Open-Drain, Digital
;	P1.7  -  NC,		Push-Pull,  Digital
	MOV	P0MDOUT,	#0CFH
	MOV	P1MDOUT,	#08FH
;	MOV	P1SKIP,   #0F0H
	MOV	XBR0,   	#001H
	MOV	XBR1,   	#040H
;---------������ʼ��
	MOV	OSCICN, #083H	;�ڲ��������ƼĴ�����SYSCLKΪ�ڲ�����1��Ƶ=24.5MHz
;---------�ɱ�̼�����/��ʱ�����г�ʼ����25mS��ʱ����
	ANL	PCA0MD, #0BFH	;�ؿ��Ź�
	MOV	PCA0MD, #000H	;ϵͳʱ�ӵ�12��Ƶ����ֹCF�ж�
	MOV	PCA0L, #9EH	;PCA��ֵ��Լ50mS
	MOV	PCA0H, #38H
;---------UART��ʼ��
	MOV	SCON0, #10H	;8λUART��ֹͣλ���߼���ƽ�����ԡ�UART��������
;---------ADC��ʼ��
;ADC0STA״̬�Ĵ���: AD0BUSY,AD0CBSY,AD0INT,AD0S3C,AD0FFC,AD0CALC,AD0ERR,AD0OVR
	MOV	ADC0MD, #080H	;ADC0ʹ��λ, ����
;	MOV       ADC0CN, #17H	;˫���ԣ�Burnout����Դ��ֹ��128�����棨һ�ϣ�
;	MOV       ADC0CN, #16H	;˫���ԣ�Burnout����Դ��ֹ��64������(50��)
	MOV	ADC0CN, #015H	;˫���ԣ�Burnout����Դ��ֹ��32������
;	MOV	ADC0CF, #0	;���üĴ������ж�ԴΪSINC3�˲������ڲ��ο�Դ��ȱʡֵ��
	MOV	ADC0CLK, #009H	;������ʱ�ӷ�Ƶ�Ĵ�����ϵͳʱ�ӵ�5��Ƶ(2.45MHz)
	MOV	ADC0DECH, #7	;ADC0��ȡ�ȼĴ������ֽڣ�ADCת�����ڣ�9Hz��ȱʡֵ��	
;	MOV	ADC0DECL, #0FFH	;ADC0��ȡ�ȼĴ������ֽ�	
;	MOV	ADC0MUX, #001H	;ģ���·���ؿ��ƼĴ�����ѹ��������1����ͨ��
	MOV	ADC0MUX, #023H	;ģ���·���ؿ��ƼĴ�����ѹ��������2����ͨ��
	MOV	ADC0MD, #081H	;ȫ�ڲ�У׼��ƫ�ƺ����棩��
	CLR	6EH		;У��ѹ��������ͨ��1
;---------�жϳ�ʼ��
	MOV	EIE1, #088H	;��ADC0��T3�ж�
	MOV	IT01CF, #006H	;INT0-�͵�ƽ��Ч���˿�P0.6
	MOV	IE, #0B1H		;��ȫ�֡�T2�������жϺ�ET0
;	MOV	EIP1, #080H	;��ʱ��T3�ж�����
;---------��־λ�����ݻ�������ʼ��
	CLR	68H		;��������·����־
	CLR	69H		;ѹ��������1����ͨ����·��־��0-������1-����
	CLR	6AH		;ѹ��������2����ͨ����·��־��0-������1-����
	CLR	6BH		;�ɼ���־��0-ֹͣ��1-��ʼ
	CLR	6CH		;¯�µ����趨�¶ȱ�־��0-δ���¶ȣ�1-����
	CLR	6DH		;����ʱ���ѹ��������1��2��־��0-ѹ��1��1-ѹ��2
	MOV	A, #0FH
	MOV	2CH, A		;�ص�ŷ�1,2,3,��ѹ��
	ORL	P0, A
	SETB	70H		;�����ѽ���ﴫ������
	CLR	71H
	MOV	50H, #"D"		;����ѹ��������1�������ݻ�����
	MOV	51H, #0
	MOV	52H, #0
	MOV	53H, #0
	MOV	54H, #0DH  
	MOV	58H, #"H"		;����ѹ��������2��������
	MOV	59H, #0
	MOV	5AH, #0
	MOV	5BH, #0
	MOV	5CH, #0DH
;---------------------------------------
;	������, MAIN
;---------------------------------------
MAIN:	SJMP	$
;**********************************************************************
;	�ⲿ�ж�0�������		<NAME: INTE0>
;**********************************************************************
INTE0:	JB	6CH, INTE01	;¯�µ����趨�¶ȣ�
	RETI			;NO, �˳��жϡ�
INTE01:	jnb	p0.6, $
	JB	6BH, INTE0E	;YES, �Ѿ���ʼ���ԣ�
	SETB	6BH		;  NO����ʼ����
	CLR	TR2		;  ֹͣT2
	MOV	TMR3H, #038H
	MOV	TMR3L, #09EH
	MOV	TMR3CN, #004H	;  ��T3
	MOV	SBUF0, #"R"	;  ���������͡�R000"
	JNB	TI0, $
	CLR	TI0
	CLR	A
	MOV	SBUF0, A
	JNB	TI0, $
	CLR	TI0
	MOV	SBUF0, A
	JNB	TI0, $
	CLR	TI0
	MOV	SBUF0, A
	JNB	TI0, $
	CLR	TI0
	MOV	SBUF0, #0DH
	JNB	TI0, $
	CLR	TI0
;	RETI
;INTE03:	CLR	6BH		;  YES����������
;	MOV	SBUF0, #"C"	;  ���������͡�C000"
;	JNB	TI0, $
;	CLR	TI0
;	CLR	A
;	MOV	SBUF0, A
;	JNB	TI0, $
;	CLR	TI0
;	MOV	SBUF0, A
;	JNB	TI0, $
;	CLR	TI0
;	MOV	SBUF0, A
;	JNB	TI0, $
;	CLR	TI0
;	MOV	SBUF0, #0DH
;	JNB	TI0, $
;	CLR	TI0
;	SETB	EV1		;�رտ�ѹ���͵�ŷ�
;	SETB	EV2
;	SETB	EV3
;	SETB	ACP
;	MOV	TMR3CN, #0	;ֹͣT3
;	MOV	TMR2H, #038H
;	MOV	TMR2L, #09EH
;	MOV	60H, #20		;ѹ����������·��ⶨʱ��20S
;	MOV	61H, #40		;������ʱ��1S
;	CLR	68H		;��������·����־
;	CLR	69H		;ѹ��������1����ͨ����·��־��0-������1-����
;	CLR	6AH		;ѹ��������2����ͨ����·��־��0-������1-����
;	CLR	6DH		;ѹ��������1
;	MOV	ADC0MD, #080H	;ADC0ʹ��λ, ����
;	MOV       ADC0CN, #17H	;˫���ԣ�Burnout����Դ��ֹ��128������
;	MOV	ADC0MUX, #001H	;ģ���·���ؿ��ƼĴ�����ѹ��������1����ͨ��
;	MOV	ADC0CGH, CG1
;	MOV	ADC0CGM, CG1+1
;	MOV	ADC0CGL, CG1+2
;	MOV	ADC0COH, CO1
;	MOV	ADC0COM, CO1+1
;	MOV	ADC0COL, CO1+2
;	MOV       ADC0MD,#83H	;ADC��ʽ�Ĵ�����ADC0ʹ�ܡ�����ת��
;	SETB	TR2		;����T2
INTE0E:	RETI
;**********************************************************************
;	ADC�жϷ������		<NAME: INTADC>
;**********************************************************************
INTADC:	CLR	AD0INT
	CLR	AD0CALC
	JBC	6EH, INTADC1
	MOV	CG2, ADC0CGH	;����ѹ��2ͨ������ϵ��
	MOV	CG2+1, ADC0CGM
	MOV	CG2+2, ADC0CGL
	MOV	CO2, ADC0COH	;����ѹ��2ͨ��ƫ��ϵ��
	MOV	CO2+1, ADC0COM
	MOV	CO2+2, ADC0COL
	MOV	ADC0MD, #080H	;����
;	MOV	ADC0CN, #17H	;���ƼĴ�����˫���Է�ʽ��Burnout����Դ��ֹ��PGA����128
	MOV	ADC0CN, #16H	;���ƼĴ�����˫���Է�ʽ��Burnout����Դ��ֹ��PGA����64(50��)
	MOV	ADC0MUX, #01H	;ѡ��ѹ��������1ͨ��
	MOV	ADC0MD, #081H	;��ʽ�Ĵ�����ADC0ʹ�ܡ�ȫ�ڲ�У׼��ƫ�ƺ����棩
	SETB	6EH
	RETI
INTADC1:	ANL	EIE1, #0F7H	;�ر�ADC�ж�(EIE1.3����)
	MOV	CG1, ADC0CGH	;����ѹ��1ͨ������ϵ��
	MOV	CG1+1, ADC0CGM
	MOV	CG1+2, ADC0CGL
	MOV	CO1, ADC0COH	;����ѹ��1ͨ��ƫ��ϵ��
	MOV	CO1+1, ADC0COM
	MOV	CO1+2, ADC0COL
	MOV	ADC0MD, #83H	;ADC��ʽ�Ĵ�����ADC0ʹ�ܡ�����ת��
;	SETB	TR2		;��T2�ж�
	RETI
;**********************************************************************
;	��ʱ��T2����������������ʱ�����жϷ������
;**********************************************************************
INTT2:	CLR	TF2H		;����жϱ�־
	DJNZ	61H, INT23	;T2�ĳ�ʼ��������61H=40��0.025*40=1s
	MOV	61H, #40		;1�����ʱ�䵽, ���ü���
	PUSH	ACC		;�����жϴ�����
	PUSH	B
	PUSH	PSW
	CLR	RS0		;�Ĵ�������2    RS1RS0=10
	SETB	RS1
	AJMP	AD00
INT22:	POP	PSW
	POP	B
	POP	ACC
INT23:	RETI
;------------------------------------------------------------------------------
;	�ɼ�ѹ�����������ֵ
; ���ڣ����ֽڶ����Ʋ�����PS1��PS1+1��PS1+2�С���ֵת��ΪmVֵӦ��ϵ����2500/16777215/128=0.00000116415328766
;------------------------------------------------------------------------------
AD00:	DJNZ	60H, AD000	;��·���ʱ�䵽��
	MOV	60H, #20		;ѹ����������·��ⶨʱ��20S
	SETB	68H
	MOV	ADC0MD, #080H	;׼���ɼ�ѹ��������2��ADC0ʹ��λ, ����
	MOV	ADC0CN, #01DH	;˫���ԣ�Burnout����Դʹ�ܣ�32������
	MOV	ADC0MUX, #023H	;ģ���·���ؿ��ƼĴ�����ѹ��������2����ͨ��
	MOV       ADC0MD,#83H	;ADC��ʽ�Ĵ�����ADC0ʹ�ܡ�����ת��
	SETB	6DH
	SJMP	INT22
AD000:	JNB	AD0INT, $
	CLR	AD0INT
	JNB	6DH, AD001	;ѡ��ѹ��������1��2
	CLR	6DH
	JMP	AD002
AD001:	SETB	6DH		;ѹ��������1
	JB	68H, AD0011	;  ��������·���ʱ�䵽��
	JB	69H, AD0011	;  ���������ڿ�·״̬��
	JMP	AD0014		;  NO, TO AD0014
AD0011:	MOV	A, ADC0H		;  YES��AD����ֵ������7FFFFFH��ͨ����·
	CJNE	A, #07FH, AD0012
	MOV	A, ADC0M
	XRL	A, #0FFH
	JNZ	AD0012
	MOV	A, ADC0L
	XRL	A, #0FFH
	JNZ	AD0012
	SETB	69H		;    ѹ��������1���뿪·��־��λ
	MOV	SBUF0, #"E"	;    ����������E001
	JNB	TI0, $
	CLR	TI0
	SJMP	AD0013
AD0012:	CLR	69H		;    ѹ��������1����ͨ������
	MOV	SBUF0, #"O"	;    ����������O001
	JNB	TI0, $
	CLR	TI0
AD0013:	MOV	SBUF0, #"0"
	JNB	TI0, $
	CLR	TI0
	MOV	SBUF0, #"0"
	JNB	TI0, $
	CLR	TI0
	MOV	SBUF0, #"1"
	JNB	TI0, $
	CLR	TI0
	MOV	SBUF0, #0DH
	JNB	TI0, $
	CLR	TI0
	CLR	68H		;    �����·����־
	SJMP	AD0016
AD0014:	MOV	PS1, ADC0H	;  NO����AD����ֵ
	MOV	PS1+1, ADC0M
	MOV	PS1+2, ADC0L
	MOV	R0, #50H		;      ����ѹ��������1��������
	MOV	R7, #5
AD0015:	MOV	SBUF0, @R0
	INC	R0
	JNB	TI0, $
	CLR	TI0
	DJNZ	R7, AD0015
AD0016:	MOV	ADC0MD, #080H	;    ׼���ɼ�ѹ��������2��ADC0ʹ��λ, ����
	MOV	ADC0CN, #015H	;    ˫���ԣ�Burnout����Դ��ֹ��32������
	MOV	ADC0MUX, #023H	;    ģ���·���ؿ��ƼĴ�����ѹ��������2����ͨ��
	MOV	ADC0CGH, CG2
	MOV	ADC0CGM, CG2+1
	MOV	ADC0CGL, CG2+2
	MOV	ADC0COH, CO2
	MOV	ADC0COM, CO2+1
	MOV	ADC0COL, CO2+2
	JB	68H, AD0017	;    
	JNB	6AH, AD0018
AD0017:	ORL	ADC0CN, #08H	;    ���ƼĴ�����Burnout����Դʹ��
AD0018:	MOV       ADC0MD,#83H	;    NO, ADC��ʽ�Ĵ�����ADC0ʹ�ܡ�����ת��
	JMP	INT22
;---------
AD002:	JB	68H, AD0021	;ѹ��������2, ��·��⣿
	JB	6AH, AD0021
	JMP	AD0024
AD0021:	MOV	A, ADC0H		;  YES, AD����ֵ������7FFFFFH��ͨ����·
	CJNE	A, #07FH, AD0022
	MOV	A, ADC0M
	XRL	A, #0FFH
	JNZ	AD0022
	MOV	A, ADC0L
	XRL	A, #0FFH
	JNZ	AD0022
	SETB	6AH		;    ѹ��������2���뿪·��־��λ
	MOV	SBUF0, #"E"	;    ����������E002
	JNB	TI0, $
	CLR	TI0
	SJMP	AD0023
AD0022:	CLR	6AH		;    ѹ��������2����ͨ������
	MOV	SBUF0, #"O"	;    ����������O002
	JNB	TI0, $
	CLR	TI0
AD0023:	MOV	SBUF0, #"0"
	JNB	TI0, $
	CLR	TI0
	MOV	SBUF0, #"0"
	JNB	TI0, $
	CLR	TI0
	MOV	SBUF0, #"2"
	JNB	TI0, $
	CLR	TI0
	MOV	SBUF0, #0DH
	JNB	TI0, $
	CLR	TI0
	SJMP	AD0026
AD0024:	MOV	PS2, ADC0H	;  NO, ��AD����ֵ
	MOV	PS2+1, ADC0M
	MOV	PS2+2, #0
	MOV	R0, #58H		;    ����ѹ��������2��������
	MOV	R7, #5
AD0025:	MOV	SBUF0, @R0
	INC	R0
	JNB	TI0, $
	CLR	TI0
	DJNZ	R7, AD0025
AD0026:	MOV	ADC0MD, #080H	;    ׼���ɼ�ѹ��������1��ADC0ʹ��λ, ����
;	MOV	ADC0CN, #017H	;    ˫���ԣ�Burnout����Դ��ֹ��128�����棨һ�ϣ�
	MOV	ADC0CN, #016H	;    ˫���ԣ�Burnout����Դ��ֹ��64������(50��)
	MOV	ADC0MUX, #001H	;    ģ���·���ؿ��ƼĴ�����ѹ��������1����ͨ��
	MOV	ADC0CGH, CG1
	MOV	ADC0CGM, CG1+1
	MOV	ADC0CGL, CG1+2
	MOV	ADC0COH, CO1
	MOV	ADC0COM, CO1+1
	MOV	ADC0COL, CO1+2
	JB	68H, AD0027	;    
	JNB	69H, AD0028
AD0027:	ORL	ADC0CN, #08H	;    ���ƼĴ�����Burnout����Դʹ��
AD0028:	MOV       ADC0MD,#83H	;    NO, ADC��ʽ�Ĵ�����ADC0ʹ�ܡ�����ת��
	JMP	INT22
;**********************************************************************
;	��ʱ��T3��������ʱ�����жϷ������
;**********************************************************************
INTT3:	MOV	TMR3CN, #004H	;����жϱ�־
	DJNZ	62H, INTT3E
	MOV	62H, #8		;�ж�ʱ�䣺0.025*8=0.2s
	PUSH	ACC		;�����жϴ�����
	PUSH	B
	PUSH	PSW
	SETB	RS0		;�Ĵ�������3
	SETB	RS1
	JB	70H, AD01	
	JMP	AD02
INT32:	POP	PSW
	POP	B
	POP	ACC
INTT3E:	RETI
;------------------------------------------------------------------------------
;	�ɼ�ѹ��������1����ֵ
; ���ڣ����ֽڶ����Ʋ�����PS1��PS1+1��PS1+2�С���ֵת��ΪmVֵӦ��ϵ����2500/16777215/128=0.00000116415328766
;------------------------------------------------------------------------------
AD01:	JNB	AD0INT, $
	CLR	AD0INT
	MOV	PS1, ADC0H	;��AD����ֵ
	MOV	PS1+1, ADC0M
	MOV	PS1+2, #0
	MOV       ADC0MD,#83H
	MOV	R0, #50H		;����ѹ��������1��������
	MOV	R7, #5
AD011:	MOV	SBUF0, @R0
	INC	R0
	JNB	TI0, $
	CLR	TI0
	DJNZ	R7, AD011
AD01E:	JMP	INT32
;------------------------------------------------------------------------------
;	�ɼ�100KPaѹ��ֵ
; ���ڣ����ֽڶ����Ʋ�����PS2��PS2+1��PS2+2�С���ֵת��ΪmVֵӦ��ϵ����2500/16777215/32=0.00000465661315
;------------------------------------------------------------------------------
AD02:	JNB	AD0INT, $
	CLR	AD0INT
	MOV	PS2, ADC0H	;��AD����ֵ
	MOV	PS2+1, ADC0M
	MOV	PS2+2, #0
	MOV       ADC0MD,#83H
	MOV	R0, #58H		;����ѹ��������2��������
	MOV	R7, #5
AD021:	MOV	SBUF0, @R0
	INC	R0
	JNB	TI0, $
	CLR	TI0
	DJNZ	R7, AD021
AD02E:	JMP	INT32
;**********************************************************************
;	�����жϷ������	<NAME: INTCOM>			*
;**********************************************************************
INTCOM:	JNB	RI0, $		;�����ֽڽ�����ϣ�
          CLR	RI0		;�ȵ��ֽڽ�����ϣ�
	PUSH	DPH
	PUSH	DPL
	PUSH	ACC
	PUSH	B
	PUSH	PSW
	SETB	RS0		;�Ĵ�������1    RS1RS0=01
	CLR	RS1
          MOV	A, SBUF0		;���ȵ��ֽ�
	CJNE	A, #23H, ECOMM1	;A<>#, ����һ��"?"
          JNB	RI0, $		;�����ֽڽ�����ϣ�
	CLR	RI0
	MOV	A, SBUF0		;�������ֽ�
	ANL	A, #0FH		;��ASCIIת��Ϊ�������ַ�
	MOV	B, #10
	DIV	AB
	JNZ	ECOMM1		;A>=8,����һ��"?"
	MOV	A,B
	RL	A
	MOV	DPTR, #ICOM2
	JMP	@A+DPTR
ICOM2:	AJMP	COMM0		;A=00H, ѹ��������1��ǰֵ��ѯ
	AJMP	COMM1		;A=01H��ѹ��������2��ǰֵ��ѯ
	AJMP	COMM2		;A=02H, ���Է�ʽָ��
	AJMP	COMM3		;A=03H, ��ŷ��Ϳ�ѹ������ָ��
	AJMP	COMM4		;A=04H, ��ʼ����ָ��
	AJMP	COMM5		;A=05H, ��������ָ��
          AJMP	COMM6		;A=06H, ����/��ֹ���ʹ������������ָ��
          AJMP	COMM7		;A=07H, ¯�µ����趨�¶�ָ��
          AJMP	COMM8		;A=08H, ������ѯָ��
          AJMP	COMM9		;A=09H, ������ȫ�ڲ�У��ָ��
;---------�˳������жϷ����ӳ���
ECOMM1:	MOV	SBUF0, #"?"
          JNB	TI0, $		;�ȴ��ֽڷ������
	CLR	TI0
	SJMP	ECOMM4
ECOMM3:	MOV	SBUF0, #"#"
	JNB	TI0, $
	CLR	TI0
ECOMM4:	CLR	A		;��������0
	MOV	SBUF0, A
          JNB	TI0, $		;�ȴ��ֽڷ������
	CLR	TI0
	MOV	SBUF0, A
          JNB	TI0, $		;�ȴ��ֽڷ������
	CLR	TI0
	MOV	SBUF0, A
          JNB	TI0, $		;�ȴ��ֽڷ������
	CLR	TI0
	MOV	SBUF0, #0DH
          JNB	TI0, $		;�ȴ��ֽڷ������
	CLR	TI0
ECOMM5:	POP	PSW
	POP	B
	POP	ACC
	POP	DPL
	POP	DPH
	RETI
;---------A=00H, ����ѹ��������1��ǰֵ
COMM0:	MOV	R0, #50H
	MOV	R7, #5
COMM01:	MOV	A, @R0
	MOV	SBUF0, A
	JNB	TI0, $
	CLR	TI0
	INC	R0
	DJNZ	R7, COMM01
	JMP	ECOMM5
;---------A=01H������ѹ��������2��ǰֵ
COMM1:	MOV	R0, #58H
	MOV	R7, #5
COMM11:	MOV	A, @R0
	MOV	SBUF0, A
	JNB	TI0, $
	CLR	TI0
	INC	R0
	DJNZ	R7, COMM11
	JMP	ECOMM5
;---------A=02H, ���Է�ʽָ��
COMM2:	JNB	RI0, $
	CLR	RI0
	MOV	A, SBUF0		;�����Է�ʽ�����֣�1-�������ԣ�2-����͸���Ի����ǿ�ȣ�
	MOV	C, ACC.1
	MOV	71H, C
	MOV	C, ACC.0
	MOV	70H, C
	MOV	ADC0MD, #080H	;ADC0ʹ��λ, ����
;	MOV       ADC0CN, #17H	;˫���ԣ�Burnout����Դ��ֹ��128�����棨һ�ϣ�
	MOV       ADC0CN, #16H	;˫���ԣ�Burnout����Դ��ֹ��64������(50��)
	MOV	ADC0MUX, #001H	;ģ���·���ؿ��ƼĴ�����ѹ��������1����ͨ��
	MOV	ADC0CGH, CG1
	MOV	ADC0CGM, CG1+1
	MOV	ADC0CGL, CG1+2
	MOV	ADC0COH, CO1
	MOV	ADC0COM, CO1+1
	MOV	ADC0COL, CO1+2
	JC	COMM21
	MOV	ADC0CN, #015H	;˫���ԣ�Burnout����Դ��ֹ��32������
	MOV	ADC0MUX, #023H	;ģ���·���ؿ��ƼĴ�����ѹ��������2����ͨ��
	MOV	ADC0CGH, CG2
	MOV	ADC0CGM, CG2+1
	MOV	ADC0CGL, CG2+2
	MOV	ADC0COH, CO2
	MOV	ADC0COM, CO2+1
	MOV	ADC0COL, CO2+2
COMM21:	MOV       ADC0MD,#83H	;ADC��ʽ�Ĵ�����ADC0ʹ�ܡ�����ת��
	JMP	ECOMM3
;---------A=03H����ŷ��Ϳ�ѹ������ָ��
COMM3:	JNB	RI0, $
	CLR	RI0
	MOV	A, SBUF0		;����ѹ������ŷ����ؿ����֣�0000xxxxB-��ѹ������3����2����1��
	ANL	A, #0FH
	MOV	2CH, A
	MOV	A, P0
	ANL	A, #0F0H
	ORL	A, 2CH
	MOV	P0, A		;�����P0�� 
	JMP	ECOMM3
;---------A=04H, ��ʼ����ָ��
COMM4:	SETB	6BH
	CLR	TR2		;ֹͣT2
	MOV	TMR3CN, #004H	;����жϱ�־, ����T3
	JMP	ECOMM3
;---------A=05H, ��������ָ��
COMM5:	CLR	6BH		;�ɼ���־��0-ֹͣ��1-��ʼ
	SETB	EV1		;�رտ�ѹ���͵�ŷ�
	SETB	EV2
	SETB	EV3
	SETB	ACP
	MOV	TMR3CN, #0	;ֹͣT3
	MOV	TMR2H, #038H
	MOV	TMR2L, #09EH
	MOV	60H, #20		;ѹ����������·��ⶨʱ��20S
	MOV	61H, #40		;������ʱ��1S       
	CLR	68H		;��������·����־
	CLR	69H		;ѹ��������1����ͨ����·��־��0-������1-����
	CLR	6AH		;ѹ��������2����ͨ����·��־��0-������1-����
	CLR	6DH		;ѹ��������1
	MOV	ADC0MD, #080H	;ADC0ʹ��λ, ����
;	MOV       ADC0CN, #17H	;˫���ԣ�Burnout����Դ��ֹ��128�����棨һ�ϣ�
	MOV       ADC0CN, #16H	;˫���ԣ�Burnout����Դ��ֹ��64������(50��)
	MOV	ADC0MUX, #001H	;ģ���·���ؿ��ƼĴ�����ѹ��������1����ͨ��
	MOV	ADC0CGH, CG1
	MOV	ADC0CGM, CG1+1
	MOV	ADC0CGL, CG1+2
	MOV	ADC0COH, CO1
	MOV	ADC0COM, CO1+1
	MOV	ADC0COL, CO1+2
	MOV       ADC0MD,#83H	;ADC��ʽ�Ĵ�����ADC0ʹ�ܡ�����ת��
	SETB	TR2		;����T2
	clr	6ch		;��ֹ�����İ�ť
	JMP	ECOMM3
;---------A=06H, ����/��ֹ���ʹ������������
COMM6:	CPL	TR2		;����/ֹͣT2
	JMP	ECOMM3
;---------A=07H, ¯�µ����趨�¶�ָ��
COMM7:	SETB	6CH
	JMP	ECOMM3
;---------A=08H, ������ѯָ��
COMM8:	MOV	SBUF0, #"H"
	JNB	TI0, $
	CLR	TI0
	MOV	SBUF0, #"S"
          JNB	TI0, $		;�ȴ��ֽڷ������
	CLR	TI0
	MOV	SBUF0, #"T"
          JNB	TI0, $		;�ȴ��ֽڷ������
	CLR	TI0
	MOV	SBUF0, #"Z"
          JNB	TI0, $		;�ȴ��ֽڷ������
	CLR	TI0
	MOV	SBUF0, #0DH
          JNB	TI0, $		;�ȴ��ֽڷ������
	CLR	TI0
	JMP	ECOMM5
;---------A=09H, ������ȫ�ڲ�У��ָ��
COMM9:	CLR	TR2		;ֹͣT2
	MOV	TMR3CN, #0	;ֹͣT3
	MOV	ADC0MD, #080H	;ADC0ʹ��λ, ����
;	MOV       ADC0CN, #17H	;˫���ԣ�Burnout����Դ��ֹ��128�����棨һ�ϣ�
	MOV       ADC0CN, #16H	;˫���ԣ�Burnout����Դ��ֹ��64������(50��)
	MOV	ADC0MUX, #001H	;ģ���·���ؿ��ƼĴ�����ѹ��������1����ͨ��
          MOV	ADC0MD, #081H	;ȫ�ڲ�У׼��ƫ�ƺ����棩��
	CLR	6EH
	JMP	ECOMM3
	END
