CREATE TABLE [dbo].[Contractor] (
    [ContractorId]   INT                                                  NOT NULL,
    [InvoiceName]    VARCHAR (50)                                         NULL,
    [InvoiceAddress] VARCHAR (250) MASKED WITH (FUNCTION = 'default()')   NULL,
    [Rate]           DECIMAL (18, 2) MASKED WITH (FUNCTION = 'default()') NULL,
    CONSTRAINT [PK_Contractor] PRIMARY KEY CLUSTERED ([ContractorId] ASC)
);

