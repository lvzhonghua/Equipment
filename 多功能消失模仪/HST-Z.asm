$include (c8051f350.inc)
PS1	DATA	51H		;压力传感器1采样值
PS2	DATA	59H		;压力传感器2采样值
CG1	DATA	70H		;通道1增益校正系数
CO1	DATA	73H		;通道1偏移校正系数
CG2	DATA	76H		;通道2增益校正系数
CO2	DATA	79H		;通道2偏移校正系数
EV1	BIT	P0.0		;电磁阀1
EV2	BIT	P0.1		;电磁阀2
EV3	BIT	P0.2		;电磁阀3
ACP	BIT	P0.3		;空压机控制位
RUN	BIT	P0.6		;运行/停止按键
;---------初始化程序
	CSEG	AT	0000H
 	LJMP	80H
	CSEG	AT	0003H
	LJMP      INTE0		;外部中断0
	CSEG	AT	000BH
	RETI			;定时器0中断
	CSEG	AT	0023H
	LJMP	INTCOM		;串口中断		
	CSEG	AT	002BH	
	LJMP	INTT2		;定时器2中断
	CSEG	AT	003BH
	RETI			;SMBus中断
	CSEG	AT	0053H
	LJMP	INTADC		;ADC0中断
	CSEG	AT	0073H
	LJMP	INTT3		;TR3中断
	DW	0,0,0,0,0

	CSEG	AT	0080H  
	MOV	SP, #0BFH		;栈底
	CLR	RS0		;寄存器区：0
	CLR	RS1
	CLR	EA		;CPU屏蔽所有的中断申请
;---------定时器T0（52.8μs时钟发生器，第二串口时钟发生器19200bps）初始化
	MOV	TMOD, #22H	;定时器T0，T1-自动重装载的8位定时器
	MOV	CKCON, #0AH	;定时器T0-系统时钟的48分频；T1-系统时钟
	MOV	TH0, #0E6H	;定时时间：52.8μs
	MOV	TL0, #0E6H		
;---------定时器T1（UART波特率发生器）初始化
	MOV	TH1, #096H	;自动重装值
	MOV	TL1, #096H	;UART波特率约115200bps
 	SETB	TR1		;开串口时钟 
;---------定时器T2（传感器零点采样定时器）初始化
	MOV	TMR2RLH, #038H	;自动重装值，中断定时约25mS
	MOV	TMR2RLL, #09EH
	MOV	TMR2H, #038H
	MOV	TMR2L, #09EH
	MOV	60H, #10		;压力传感器开路检测定时：10S
	MOV	61H, #40		;采样定时：1S
	CLR	6DH		;压力传感器1
;---------定时器T3（采样定时器）初始化
	MOV	TMR3RLH, #038H	;自动重装值，中断定时约25mS
	MOV	TMR3RLL, #09EH
	MOV	TMR3H, #038H
	MOV	TMR3L, #09EH
	MOV	TMR3CN, #00H	;16位自动重装，中断标志清零，关闭T3
	MOV	62H, #8
;---------IO口初始化
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
;---------振荡器初始化
	MOV	OSCICN, #083H	;内部振荡器控制寄存器：SYSCLK为内部振荡器1分频=24.5MHz
;---------可编程计数器/定时器阵列初始化（25mS定时器）
	ANL	PCA0MD, #0BFH	;关看门狗
	MOV	PCA0MD, #000H	;系统时钟的12分频，禁止CF中断
	MOV	PCA0L, #9EH	;PCA初值：约50mS
	MOV	PCA0H, #38H
;---------UART初始化
	MOV	SCON0, #10H	;8位UART、停止位的逻辑电平被忽略、UART接收允许
;---------ADC初始化
;ADC0STA状态寄存器: AD0BUSY,AD0CBSY,AD0INT,AD0S3C,AD0FFC,AD0CALC,AD0ERR,AD0OVR
	MOV	ADC0MD, #080H	;ADC0使能位, 空闲
;	MOV       ADC0CN, #17H	;双极性，Burnout电流源禁止，128倍增益（一拖）
;	MOV       ADC0CN, #16H	;双极性，Burnout电流源禁止，64倍增益(50厂)
	MOV	ADC0CN, #015H	;双极性，Burnout电流源禁止，32倍增益
;	MOV	ADC0CF, #0	;配置寄存器：中断源为SINC3滤波器、内部参考源（缺省值）
	MOV	ADC0CLK, #009H	;调制器时钟分频寄存器：系统时钟的5分频(2.45MHz)
	MOV	ADC0DECH, #7	;ADC0抽取比寄存器高字节，ADC转换周期：9Hz（缺省值）	
;	MOV	ADC0DECL, #0FFH	;ADC0抽取比寄存器低字节	
;	MOV	ADC0MUX, #001H	;模拟多路开关控制寄存器：压力传感器1输入通道
	MOV	ADC0MUX, #023H	;模拟多路开关控制寄存器：压力传感器2输入通道
	MOV	ADC0MD, #081H	;全内部校准（偏移和增益）。
	CLR	6EH		;校验压力传感器通道1
;---------中断初始化
	MOV	EIE1, #088H	;开ADC0，T3中断
	MOV	IT01CF, #006H	;INT0-低电平有效；端口P0.6
	MOV	IE, #0B1H		;开全局、T2、串口中断和ET0
;	MOV	EIP1, #080H	;定时器T3中断优先
;---------标志位及数据缓存区初始化
	CLR	68H		;传感器开路检测标志
	CLR	69H		;压力传感器1输入通道开路标志，0-正常；1-出错
	CLR	6AH		;压力传感器2输入通道开路标志，0-正常；1-出错
	CLR	6BH		;采集标志，0-停止；1-开始
	CLR	6CH		;炉温到达设定温度标志。0-未到温度；1-到达
	CLR	6DH		;空闲时检测压力传感器1或2标志，0-压力1；1-压力2
	MOV	A, #0FH
	MOV	2CH, A		;关电磁阀1,2,3,空压机
	ORL	P0, A
	SETB	70H		;测试裂解产物传输特性
	CLR	71H
	MOV	50H, #"D"		;设置压力传感器1采样数据缓冲区
	MOV	51H, #0
	MOV	52H, #0
	MOV	53H, #0
	MOV	54H, #0DH  
	MOV	58H, #"H"		;设置压力传感器2采样数据
	MOV	59H, #0
	MOV	5AH, #0
	MOV	5BH, #0
	MOV	5CH, #0DH
;---------------------------------------
;	主程序, MAIN
;---------------------------------------
MAIN:	SJMP	$
;**********************************************************************
;	外部中断0服务程序		<NAME: INTE0>
;**********************************************************************
INTE0:	JB	6CH, INTE01	;炉温到达设定温度？
	RETI			;NO, 退出中断。
INTE01:	jnb	p0.6, $
	JB	6BH, INTE0E	;YES, 已经开始测试？
	SETB	6BH		;  NO，开始测试
	CLR	TR2		;  停止T2
	MOV	TMR3H, #038H
	MOV	TMR3L, #09EH
	MOV	TMR3CN, #004H	;  开T3
	MOV	SBUF0, #"R"	;  向主机发送“R000"
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
;INTE03:	CLR	6BH		;  YES，结束测试
;	MOV	SBUF0, #"C"	;  向主机发送“C000"
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
;	SETB	EV1		;关闭空压机和电磁阀
;	SETB	EV2
;	SETB	EV3
;	SETB	ACP
;	MOV	TMR3CN, #0	;停止T3
;	MOV	TMR2H, #038H
;	MOV	TMR2L, #09EH
;	MOV	60H, #20		;压力传感器开路检测定时：20S
;	MOV	61H, #40		;采样定时：1S
;	CLR	68H		;传感器开路检测标志
;	CLR	69H		;压力传感器1输入通道开路标志，0-正常；1-出错
;	CLR	6AH		;压力传感器2输入通道开路标志，0-正常；1-出错
;	CLR	6DH		;压力传感器1
;	MOV	ADC0MD, #080H	;ADC0使能位, 空闲
;	MOV       ADC0CN, #17H	;双极性，Burnout电流源禁止，128倍增益
;	MOV	ADC0MUX, #001H	;模拟多路开关控制寄存器：压力传感器1输入通道
;	MOV	ADC0CGH, CG1
;	MOV	ADC0CGM, CG1+1
;	MOV	ADC0CGL, CG1+2
;	MOV	ADC0COH, CO1
;	MOV	ADC0COM, CO1+1
;	MOV	ADC0COL, CO1+2
;	MOV       ADC0MD,#83H	;ADC方式寄存器：ADC0使能、连续转换
;	SETB	TR2		;启动T2
INTE0E:	RETI
;**********************************************************************
;	ADC中断服务程序		<NAME: INTADC>
;**********************************************************************
INTADC:	CLR	AD0INT
	CLR	AD0CALC
	JBC	6EH, INTADC1
	MOV	CG2, ADC0CGH	;保存压力2通道增益系数
	MOV	CG2+1, ADC0CGM
	MOV	CG2+2, ADC0CGL
	MOV	CO2, ADC0COH	;保存压力2通道偏移系数
	MOV	CO2+1, ADC0COM
	MOV	CO2+2, ADC0COL
	MOV	ADC0MD, #080H	;空闲
;	MOV	ADC0CN, #17H	;控制寄存器：双极性方式、Burnout电流源禁止、PGA增益128
	MOV	ADC0CN, #16H	;控制寄存器：双极性方式、Burnout电流源禁止、PGA增益64(50厂)
	MOV	ADC0MUX, #01H	;选择压力传感器1通道
	MOV	ADC0MD, #081H	;方式寄存器：ADC0使能、全内部校准（偏移和增益）
	SETB	6EH
	RETI
INTADC1:	ANL	EIE1, #0F7H	;关闭ADC中断(EIE1.3清零)
	MOV	CG1, ADC0CGH	;保存压力1通道增益系数
	MOV	CG1+1, ADC0CGM
	MOV	CG1+2, ADC0CGL
	MOV	CO1, ADC0COH	;保存压力1通道偏移系数
	MOV	CO1+1, ADC0COM
	MOV	CO1+2, ADC0COL
	MOV	ADC0MD, #83H	;ADC方式寄存器：ADC0使能、连续转换
;	SETB	TR2		;开T2中断
	RETI
;**********************************************************************
;	定时器T2（传感器零点采样定时器）中断服务程序
;**********************************************************************
INTT2:	CLR	TF2H		;清除中断标志
	DJNZ	61H, INT23	;T2的初始化程序中61H=40，0.025*40=1s
	MOV	61H, #40		;1秒计数时间到, 重置计数
	PUSH	ACC		;进入中断处理部分
	PUSH	B
	PUSH	PSW
	CLR	RS0		;寄存器区：2    RS1RS0=10
	SETB	RS1
	AJMP	AD00
INT22:	POP	PSW
	POP	B
	POP	ACC
INT23:	RETI
;------------------------------------------------------------------------------
;	采集压力传感器零点值
; 出口：三字节二进制补码在PS1、PS1+1和PS1+2中。该值转换为mV值应乘系数：2500/16777215/128=0.00000116415328766
;------------------------------------------------------------------------------
AD00:	DJNZ	60H, AD000	;开路检测时间到？
	MOV	60H, #20		;压力传感器开路检测定时：20S
	SETB	68H
	MOV	ADC0MD, #080H	;准备采集压力传感器2，ADC0使能位, 空闲
	MOV	ADC0CN, #01DH	;双极性，Burnout电流源使能，32倍增益
	MOV	ADC0MUX, #023H	;模拟多路开关控制寄存器：压力传感器2输入通道
	MOV       ADC0MD,#83H	;ADC方式寄存器：ADC0使能、连续转换
	SETB	6DH
	SJMP	INT22
AD000:	JNB	AD0INT, $
	CLR	AD0INT
	JNB	6DH, AD001	;选择压力传感器1或2
	CLR	6DH
	JMP	AD002
AD001:	SETB	6DH		;压力传感器1
	JB	68H, AD0011	;  传感器开路检测时间到？
	JB	69H, AD0011	;  传感器处于开路状态？
	JMP	AD0014		;  NO, TO AD0014
AD0011:	MOV	A, ADC0H		;  YES，AD采样值若等于7FFFFFH，通道开路
	CJNE	A, #07FH, AD0012
	MOV	A, ADC0M
	XRL	A, #0FFH
	JNZ	AD0012
	MOV	A, ADC0L
	XRL	A, #0FFH
	JNZ	AD0012
	SETB	69H		;    压力传感器1输入开路标志置位
	MOV	SBUF0, #"E"	;    向主机发送E001
	JNB	TI0, $
	CLR	TI0
	SJMP	AD0013
AD0012:	CLR	69H		;    压力传感器1输入通道正常
	MOV	SBUF0, #"O"	;    向主机发送O001
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
	CLR	68H		;    清除开路检测标志
	SJMP	AD0016
AD0014:	MOV	PS1, ADC0H	;  NO，读AD采样值
	MOV	PS1+1, ADC0M
	MOV	PS1+2, ADC0L
	MOV	R0, #50H		;      发送压力传感器1采样数据
	MOV	R7, #5
AD0015:	MOV	SBUF0, @R0
	INC	R0
	JNB	TI0, $
	CLR	TI0
	DJNZ	R7, AD0015
AD0016:	MOV	ADC0MD, #080H	;    准备采集压力传感器2，ADC0使能位, 空闲
	MOV	ADC0CN, #015H	;    双极性，Burnout电流源禁止，32倍增益
	MOV	ADC0MUX, #023H	;    模拟多路开关控制寄存器：压力传感器2输入通道
	MOV	ADC0CGH, CG2
	MOV	ADC0CGM, CG2+1
	MOV	ADC0CGL, CG2+2
	MOV	ADC0COH, CO2
	MOV	ADC0COM, CO2+1
	MOV	ADC0COL, CO2+2
	JB	68H, AD0017	;    
	JNB	6AH, AD0018
AD0017:	ORL	ADC0CN, #08H	;    控制寄存器：Burnout电流源使能
AD0018:	MOV       ADC0MD,#83H	;    NO, ADC方式寄存器：ADC0使能、连续转换
	JMP	INT22
;---------
AD002:	JB	68H, AD0021	;压力传感器2, 开路检测？
	JB	6AH, AD0021
	JMP	AD0024
AD0021:	MOV	A, ADC0H		;  YES, AD采样值若等于7FFFFFH，通道开路
	CJNE	A, #07FH, AD0022
	MOV	A, ADC0M
	XRL	A, #0FFH
	JNZ	AD0022
	MOV	A, ADC0L
	XRL	A, #0FFH
	JNZ	AD0022
	SETB	6AH		;    压力传感器2输入开路标志置位
	MOV	SBUF0, #"E"	;    向主机发送E002
	JNB	TI0, $
	CLR	TI0
	SJMP	AD0023
AD0022:	CLR	6AH		;    压力传感器2输入通道正常
	MOV	SBUF0, #"O"	;    向主机发送O002
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
AD0024:	MOV	PS2, ADC0H	;  NO, 读AD采样值
	MOV	PS2+1, ADC0M
	MOV	PS2+2, #0
	MOV	R0, #58H		;    发送压力传感器2采样数据
	MOV	R7, #5
AD0025:	MOV	SBUF0, @R0
	INC	R0
	JNB	TI0, $
	CLR	TI0
	DJNZ	R7, AD0025
AD0026:	MOV	ADC0MD, #080H	;    准备采集压力传感器1，ADC0使能位, 空闲
;	MOV	ADC0CN, #017H	;    双极性，Burnout电流源禁止，128倍增益（一拖）
	MOV	ADC0CN, #016H	;    双极性，Burnout电流源禁止，64倍增益(50厂)
	MOV	ADC0MUX, #001H	;    模拟多路开关控制寄存器：压力传感器1输入通道
	MOV	ADC0CGH, CG1
	MOV	ADC0CGM, CG1+1
	MOV	ADC0CGL, CG1+2
	MOV	ADC0COH, CO1
	MOV	ADC0COM, CO1+1
	MOV	ADC0COL, CO1+2
	JB	68H, AD0027	;    
	JNB	69H, AD0028
AD0027:	ORL	ADC0CN, #08H	;    控制寄存器：Burnout电流源使能
AD0028:	MOV       ADC0MD,#83H	;    NO, ADC方式寄存器：ADC0使能、连续转换
	JMP	INT22
;**********************************************************************
;	定时器T3（采样定时器）中断服务程序
;**********************************************************************
INTT3:	MOV	TMR3CN, #004H	;清除中断标志
	DJNZ	62H, INTT3E
	MOV	62H, #8		;中断时间：0.025*8=0.2s
	PUSH	ACC		;进入中断处理部分
	PUSH	B
	PUSH	PSW
	SETB	RS0		;寄存器区：3
	SETB	RS1
	JB	70H, AD01	
	JMP	AD02
INT32:	POP	PSW
	POP	B
	POP	ACC
INTT3E:	RETI
;------------------------------------------------------------------------------
;	采集压力传感器1采样值
; 出口：三字节二进制补码在PS1、PS1+1和PS1+2中。该值转换为mV值应乘系数：2500/16777215/128=0.00000116415328766
;------------------------------------------------------------------------------
AD01:	JNB	AD0INT, $
	CLR	AD0INT
	MOV	PS1, ADC0H	;读AD采样值
	MOV	PS1+1, ADC0M
	MOV	PS1+2, #0
	MOV       ADC0MD,#83H
	MOV	R0, #50H		;发送压力传感器1采样数据
	MOV	R7, #5
AD011:	MOV	SBUF0, @R0
	INC	R0
	JNB	TI0, $
	CLR	TI0
	DJNZ	R7, AD011
AD01E:	JMP	INT32
;------------------------------------------------------------------------------
;	采集100KPa压力值
; 出口：三字节二进制补码在PS2、PS2+1和PS2+2中。该值转换为mV值应乘系数：2500/16777215/32=0.00000465661315
;------------------------------------------------------------------------------
AD02:	JNB	AD0INT, $
	CLR	AD0INT
	MOV	PS2, ADC0H	;读AD采样值
	MOV	PS2+1, ADC0M
	MOV	PS2+2, #0
	MOV       ADC0MD,#83H
	MOV	R0, #58H		;发送压力传感器2采样数据
	MOV	R7, #5
AD021:	MOV	SBUF0, @R0
	INC	R0
	JNB	TI0, $
	CLR	TI0
	DJNZ	R7, AD021
AD02E:	JMP	INT32
;**********************************************************************
;	串行中断服务程序	<NAME: INTCOM>			*
;**********************************************************************
INTCOM:	JNB	RI0, $		;命令字节接收完毕？
          CLR	RI0		;先导字节接收完毕？
	PUSH	DPH
	PUSH	DPL
	PUSH	ACC
	PUSH	B
	PUSH	PSW
	SETB	RS0		;寄存器区：1    RS1RS0=01
	CLR	RS1
          MOV	A, SBUF0		;读先导字节
	CJNE	A, #23H, ECOMM1	;A<>#, 返回一个"?"
          JNB	RI0, $		;命令字节接收完毕？
	CLR	RI0
	MOV	A, SBUF0		;读命令字节
	ANL	A, #0FH		;将ASCII转换为二进制字符
	MOV	B, #10
	DIV	AB
	JNZ	ECOMM1		;A>=8,返回一个"?"
	MOV	A,B
	RL	A
	MOV	DPTR, #ICOM2
	JMP	@A+DPTR
ICOM2:	AJMP	COMM0		;A=00H, 压力传感器1当前值查询
	AJMP	COMM1		;A=01H，压力传感器2当前值查询
	AJMP	COMM2		;A=02H, 测试方式指令
	AJMP	COMM3		;A=03H, 电磁阀和空压机控制指令
	AJMP	COMM4		;A=04H, 开始测试指令
	AJMP	COMM5		;A=05H, 结束测试指令
          AJMP	COMM6		;A=06H, 允许/禁止发送传感器零点数据指令
          AJMP	COMM7		;A=07H, 炉温到达设定温度指令
          AJMP	COMM8		;A=08H, 仪器查询指令
          AJMP	COMM9		;A=09H, 传感器全内部校正指令
;---------退出串行中断服务子程序
ECOMM1:	MOV	SBUF0, #"?"
          JNB	TI0, $		;等待字节发送完毕
	CLR	TI0
	SJMP	ECOMM4
ECOMM3:	MOV	SBUF0, #"#"
	JNB	TI0, $
	CLR	TI0
ECOMM4:	CLR	A		;发送三个0
	MOV	SBUF0, A
          JNB	TI0, $		;等待字节发送完毕
	CLR	TI0
	MOV	SBUF0, A
          JNB	TI0, $		;等待字节发送完毕
	CLR	TI0
	MOV	SBUF0, A
          JNB	TI0, $		;等待字节发送完毕
	CLR	TI0
	MOV	SBUF0, #0DH
          JNB	TI0, $		;等待字节发送完毕
	CLR	TI0
ECOMM5:	POP	PSW
	POP	B
	POP	ACC
	POP	DPL
	POP	DPH
	RETI
;---------A=00H, 发送压力传感器1当前值
COMM0:	MOV	R0, #50H
	MOV	R7, #5
COMM01:	MOV	A, @R0
	MOV	SBUF0, A
	JNB	TI0, $
	CLR	TI0
	INC	R0
	DJNZ	R7, COMM01
	JMP	ECOMM5
;---------A=01H，发送压力传感器2当前值
COMM1:	MOV	R0, #58H
	MOV	R7, #5
COMM11:	MOV	A, @R0
	MOV	SBUF0, A
	JNB	TI0, $
	CLR	TI0
	INC	R0
	DJNZ	R7, COMM11
	JMP	ECOMM5
;---------A=02H, 测试方式指令
COMM2:	JNB	RI0, $
	CLR	RI0
	MOV	A, SBUF0		;读测试方式控制字（1-传输特性；2-高温透气性或高温强度）
	MOV	C, ACC.1
	MOV	71H, C
	MOV	C, ACC.0
	MOV	70H, C
	MOV	ADC0MD, #080H	;ADC0使能位, 空闲
;	MOV       ADC0CN, #17H	;双极性，Burnout电流源禁止，128倍增益（一拖）
	MOV       ADC0CN, #16H	;双极性，Burnout电流源禁止，64倍增益(50厂)
	MOV	ADC0MUX, #001H	;模拟多路开关控制寄存器：压力传感器1输入通道
	MOV	ADC0CGH, CG1
	MOV	ADC0CGM, CG1+1
	MOV	ADC0CGL, CG1+2
	MOV	ADC0COH, CO1
	MOV	ADC0COM, CO1+1
	MOV	ADC0COL, CO1+2
	JC	COMM21
	MOV	ADC0CN, #015H	;双极性，Burnout电流源禁止，32倍增益
	MOV	ADC0MUX, #023H	;模拟多路开关控制寄存器：压力传感器2输入通道
	MOV	ADC0CGH, CG2
	MOV	ADC0CGM, CG2+1
	MOV	ADC0CGL, CG2+2
	MOV	ADC0COH, CO2
	MOV	ADC0COM, CO2+1
	MOV	ADC0COL, CO2+2
COMM21:	MOV       ADC0MD,#83H	;ADC方式寄存器：ADC0使能、连续转换
	JMP	ECOMM3
;---------A=03H，电磁阀和空压机控制指令
COMM3:	JNB	RI0, $
	CLR	RI0
	MOV	A, SBUF0		;读空压机，电磁阀开关控制字（0000xxxxB-空压机，阀3，阀2，阀1）
	ANL	A, #0FH
	MOV	2CH, A
	MOV	A, P0
	ANL	A, #0F0H
	ORL	A, 2CH
	MOV	P0, A		;输出到P0口 
	JMP	ECOMM3
;---------A=04H, 开始测试指令
COMM4:	SETB	6BH
	CLR	TR2		;停止T2
	MOV	TMR3CN, #004H	;清除中断标志, 启动T3
	JMP	ECOMM3
;---------A=05H, 结束测试指令
COMM5:	CLR	6BH		;采集标志，0-停止；1-开始
	SETB	EV1		;关闭空压机和电磁阀
	SETB	EV2
	SETB	EV3
	SETB	ACP
	MOV	TMR3CN, #0	;停止T3
	MOV	TMR2H, #038H
	MOV	TMR2L, #09EH
	MOV	60H, #20		;压力传感器开路检测定时：20S
	MOV	61H, #40		;采样定时：1S       
	CLR	68H		;传感器开路检测标志
	CLR	69H		;压力传感器1输入通道开路标志，0-正常；1-出错
	CLR	6AH		;压力传感器2输入通道开路标志，0-正常；1-出错
	CLR	6DH		;压力传感器1
	MOV	ADC0MD, #080H	;ADC0使能位, 空闲
;	MOV       ADC0CN, #17H	;双极性，Burnout电流源禁止，128倍增益（一拖）
	MOV       ADC0CN, #16H	;双极性，Burnout电流源禁止，64倍增益(50厂)
	MOV	ADC0MUX, #001H	;模拟多路开关控制寄存器：压力传感器1输入通道
	MOV	ADC0CGH, CG1
	MOV	ADC0CGM, CG1+1
	MOV	ADC0CGL, CG1+2
	MOV	ADC0COH, CO1
	MOV	ADC0COM, CO1+1
	MOV	ADC0COL, CO1+2
	MOV       ADC0MD,#83H	;ADC方式寄存器：ADC0使能、连续转换
	SETB	TR2		;启动T2
	clr	6ch		;禁止启动的按钮
	JMP	ECOMM3
;---------A=06H, 允许/禁止发送传感器零点数据
COMM6:	CPL	TR2		;启动/停止T2
	JMP	ECOMM3
;---------A=07H, 炉温到达设定温度指令
COMM7:	SETB	6CH
	JMP	ECOMM3
;---------A=08H, 仪器查询指令
COMM8:	MOV	SBUF0, #"H"
	JNB	TI0, $
	CLR	TI0
	MOV	SBUF0, #"S"
          JNB	TI0, $		;等待字节发送完毕
	CLR	TI0
	MOV	SBUF0, #"T"
          JNB	TI0, $		;等待字节发送完毕
	CLR	TI0
	MOV	SBUF0, #"Z"
          JNB	TI0, $		;等待字节发送完毕
	CLR	TI0
	MOV	SBUF0, #0DH
          JNB	TI0, $		;等待字节发送完毕
	CLR	TI0
	JMP	ECOMM5
;---------A=09H, 传感器全内部校正指令
COMM9:	CLR	TR2		;停止T2
	MOV	TMR3CN, #0	;停止T3
	MOV	ADC0MD, #080H	;ADC0使能位, 空闲
;	MOV       ADC0CN, #17H	;双极性，Burnout电流源禁止，128倍增益（一拖）
	MOV       ADC0CN, #16H	;双极性，Burnout电流源禁止，64倍增益(50厂)
	MOV	ADC0MUX, #001H	;模拟多路开关控制寄存器：压力传感器1输入通道
          MOV	ADC0MD, #081H	;全内部校准（偏移和增益）。
	CLR	6EH
	JMP	ECOMM3
	END
