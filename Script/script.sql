USE [Test_Invoice]
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 2/20/2023 2:15:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustName] [nvarchar](70) NOT NULL,
	[Adress] [nvarchar](120) NOT NULL,
	[Status] [bit] NOT NULL,
	[CustomerTypeId] [int] NOT NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerTypes]    Script Date: 2/20/2023 2:15:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](70) NOT NULL,
 CONSTRAINT [PK_CustomerType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Invoice]    Script Date: 2/20/2023 2:15:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Invoice](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[TotalItbis] [money] NOT NULL,
	[SubTotal] [money] NOT NULL,
	[Total] [money] NOT NULL,
 CONSTRAINT [PK_Invoice] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InvoiceDetail]    Script Date: 2/20/2023 2:15:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InvoiceDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[Qty] [int] NOT NULL,
	[Price] [money] NOT NULL,
	[TotalItbis] [money] NOT NULL,
	[SubTotal] [money] NOT NULL,
	[Total] [money] NOT NULL,
 CONSTRAINT [PK_InvoiceDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Customers] ON 

INSERT [dbo].[Customers] ([Id], [CustName], [Adress], [Status], [CustomerTypeId]) VALUES (1, N'Edwin Tejada', N'ASDAS', 1, 9)
INSERT [dbo].[Customers] ([Id], [CustName], [Adress], [Status], [CustomerTypeId]) VALUES (2, N'Edwin Tejada', N'ASDAS', 1, 9)
INSERT [dbo].[Customers] ([Id], [CustName], [Adress], [Status], [CustomerTypeId]) VALUES (5, N'Edwin Tejada', N'ASDAS', 1, 9)
SET IDENTITY_INSERT [dbo].[Customers] OFF
GO
SET IDENTITY_INSERT [dbo].[CustomerTypes] ON 

INSERT [dbo].[CustomerTypes] ([Id], [Description]) VALUES (9, N'Empleadoss')
INSERT [dbo].[CustomerTypes] ([Id], [Description]) VALUES (14, N'Empleado')
INSERT [dbo].[CustomerTypes] ([Id], [Description]) VALUES (19, N'Empleado')
SET IDENTITY_INSERT [dbo].[CustomerTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[Invoice] ON 

INSERT [dbo].[Invoice] ([Id], [CustomerId], [TotalItbis], [SubTotal], [Total]) VALUES (1, 1, 1.0000, 1.0000, 1.0000)
INSERT [dbo].[Invoice] ([Id], [CustomerId], [TotalItbis], [SubTotal], [Total]) VALUES (2, 2, 5.0000, 4.0000, 4.0000)
INSERT [dbo].[Invoice] ([Id], [CustomerId], [TotalItbis], [SubTotal], [Total]) VALUES (4, 1, 1.0000, 3.0000, 1.0000)
SET IDENTITY_INSERT [dbo].[Invoice] OFF
GO
SET IDENTITY_INSERT [dbo].[InvoiceDetail] ON 

INSERT [dbo].[InvoiceDetail] ([Id], [CustomerId], [Qty], [Price], [TotalItbis], [SubTotal], [Total]) VALUES (1, 1, 1, 2.0000, 1.0000, 1.0000, 1.0000)
INSERT [dbo].[InvoiceDetail] ([Id], [CustomerId], [Qty], [Price], [TotalItbis], [SubTotal], [Total]) VALUES (2, 2, 3, 4.0000, 5.0000, 4.0000, 4.0000)
INSERT [dbo].[InvoiceDetail] ([Id], [CustomerId], [Qty], [Price], [TotalItbis], [SubTotal], [Total]) VALUES (3, 2, 4, 4.0000, 4.0000, 5.0000, 4.0000)
INSERT [dbo].[InvoiceDetail] ([Id], [CustomerId], [Qty], [Price], [TotalItbis], [SubTotal], [Total]) VALUES (4, 1, 4, 4.0000, 1.0000, 3.0000, 1.0000)
SET IDENTITY_INSERT [dbo].[InvoiceDetail] OFF
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_Status]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_CustomerType]  DEFAULT ((1)) FOR [CustomerTypeId]
GO
ALTER TABLE [dbo].[Invoice] ADD  CONSTRAINT [DF_Invoice_TotalItbis]  DEFAULT ((0)) FOR [TotalItbis]
GO
ALTER TABLE [dbo].[InvoiceDetail] ADD  CONSTRAINT [DF_InvoiceDetail_TotalItbis1]  DEFAULT ((0)) FOR [Qty]
GO
ALTER TABLE [dbo].[InvoiceDetail] ADD  CONSTRAINT [DF_InvoiceDetail_TotalItbis]  DEFAULT ((0)) FOR [TotalItbis]
GO
ALTER TABLE [dbo].[Customers]  WITH CHECK ADD  CONSTRAINT [FK_Customers_CustomerTypes] FOREIGN KEY([CustomerTypeId])
REFERENCES [dbo].[CustomerTypes] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Customers] CHECK CONSTRAINT [FK_Customers_CustomerTypes]
GO
ALTER TABLE [dbo].[Invoice]  WITH CHECK ADD  CONSTRAINT [FK_Invoice_Customers] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Invoice] CHECK CONSTRAINT [FK_Invoice_Customers]
GO
ALTER TABLE [dbo].[InvoiceDetail]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceDetail_Invoice] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Invoice] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[InvoiceDetail] CHECK CONSTRAINT [FK_InvoiceDetail_Invoice]
GO
