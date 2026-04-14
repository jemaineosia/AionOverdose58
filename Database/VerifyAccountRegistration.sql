-- =============================================
-- Account Registration Verification Queries
-- Database: AionAccounts
-- =============================================

USE AionAccounts
GO

-- =============================================
-- 1. CHECK IF ACCOUNT EXISTS IN ALL TABLES
-- Replace 'YourUsername' with the actual username
-- =============================================
DECLARE @AccountName VARCHAR(14) = 'YourUsername'

PRINT '========================================='
PRINT 'Checking Account: ' + @AccountName
PRINT '========================================='
PRINT ''

-- Check SSN Table
PRINT '--- SSN Table ---'
SELECT 
    ssn,
    name,
    email,
    mobile,
    reg_date,
    account_num,
    status_flag
FROM ssn
WHERE name = @AccountName

-- Check User Account Table
PRINT ''
PRINT '--- User Account Table ---'
SELECT 
    uid,
    account,
    pay_stat
FROM user_account
WHERE account = @AccountName

-- Check User Auth Table
PRINT ''
PRINT '--- User Auth Table ---'
SELECT 
    account,
    CASE WHEN password IS NOT NULL THEN 'YES' ELSE 'NO' END AS HasPassword,
    CASE WHEN passwd IS NOT NULL THEN 'YES' ELSE 'NO' END AS HasPasswd,
    CASE WHEN web_password IS NOT NULL THEN 'YES' ELSE 'NO' END AS HasWebPassword,
    quiz1,
    quiz2,
    new_pwd_flag,
    web_level
FROM user_auth
WHERE account = @AccountName

-- Check User Info Table
PRINT ''
PRINT '--- User Info Table ---'
SELECT 
    account,
    create_date,
    ssn,
    status_flag,
    kind
FROM user_info
WHERE account = @AccountName

GO

-- =============================================
-- 2. FIND RECENTLY CREATED ACCOUNTS (Last 24 hours)
-- =============================================
PRINT ''
PRINT '========================================='
PRINT 'RECENTLY CREATED ACCOUNTS (Last 24 hrs)'
PRINT '========================================='
PRINT ''

SELECT 
    ui.account,
    ui.create_date,
    s.email,
    s.mobile AS PIN,
    ua.uid,
    ua.pay_stat,
    ui.status_flag,
    CASE WHEN uat.account IS NOT NULL THEN 'YES' ELSE 'NO' END AS HasAuth
FROM user_info ui
LEFT JOIN ssn s ON ui.ssn = s.ssn
LEFT JOIN user_account ua ON ui.account = ua.account
LEFT JOIN user_auth uat ON ui.account = uat.account
WHERE ui.create_date >= DATEADD(HOUR, -24, GETDATE())
ORDER BY ui.create_date DESC

GO

-- =============================================
-- 3. VERIFY DATA INTEGRITY - Find Incomplete Accounts
-- =============================================
PRINT ''
PRINT '========================================='
PRINT 'INCOMPLETE ACCOUNTS (Missing Records)'
PRINT '========================================='
PRINT ''

-- Accounts in user_info but not in user_account
PRINT '--- Missing in user_account ---'
SELECT ui.account, ui.create_date
FROM user_info ui
LEFT JOIN user_account ua ON ui.account = ua.account
WHERE ua.account IS NULL

PRINT ''
PRINT '--- Missing in user_auth ---'
-- Accounts in user_info but not in user_auth
SELECT ui.account, ui.create_date
FROM user_info ui
LEFT JOIN user_auth uat ON ui.account = uat.account
WHERE uat.account IS NULL

PRINT ''
PRINT '--- Missing SSN records ---'
-- Accounts in user_info but SSN record not found
SELECT ui.account, ui.ssn, ui.create_date
FROM user_info ui
LEFT JOIN ssn s ON ui.ssn = s.ssn
WHERE s.ssn IS NULL

GO

-- =============================================
-- 4. COMPLETE ACCOUNT DETAILS FOR SPECIFIC USER
-- =============================================
DECLARE @CheckAccount VARCHAR(14) = 'YourUsername'

PRINT ''
PRINT '========================================='
PRINT 'COMPLETE ACCOUNT DETAILS'
PRINT '========================================='
PRINT ''

SELECT 
    'Account Name' AS Field, ui.account AS Value
UNION ALL
SELECT 'User ID (UID)', CAST(ua.uid AS VARCHAR(50))
UNION ALL
SELECT 'Email', ISNULL(s.email, 'N/A')
UNION ALL
SELECT 'PIN/Mobile', ISNULL(s.mobile, 'N/A')
UNION ALL
SELECT 'SSN', ISNULL(ui.ssn, 'N/A')
UNION ALL
SELECT 'Created Date', CONVERT(VARCHAR(50), ui.create_date, 120)
UNION ALL
SELECT 'Registration Date', CONVERT(VARCHAR(50), s.reg_date, 120)
UNION ALL
SELECT 'Pay Status', CASE ua.pay_stat WHEN 1 THEN 'Active' ELSE 'Inactive' END
UNION ALL
SELECT 'Status Flag', CAST(ui.status_flag AS VARCHAR(50))
UNION ALL
SELECT 'Kind', CAST(ui.kind AS VARCHAR(50))
UNION ALL
SELECT 'Web Level', CAST(uat.web_level AS VARCHAR(50))
UNION ALL
SELECT 'Has Binary Password', CASE WHEN uat.password IS NOT NULL THEN 'YES' ELSE 'NO' END
UNION ALL
SELECT 'Has Web Password', CASE WHEN uat.passwd IS NOT NULL THEN 'YES' ELSE 'NO' END
UNION ALL
SELECT 'Has Hex Password', CASE WHEN uat.web_password IS NOT NULL THEN 'YES' ELSE 'NO' END
FROM user_info ui
LEFT JOIN user_account ua ON ui.account = ua.account
LEFT JOIN ssn s ON ui.ssn = s.ssn
LEFT JOIN user_auth uat ON ui.account = uat.account
WHERE ui.account = @CheckAccount

GO

-- =============================================
-- 5. COUNT TOTAL ACCOUNTS PER TABLE
-- =============================================
PRINT ''
PRINT '========================================='
PRINT 'TOTAL RECORDS IN EACH TABLE'
PRINT '========================================='
PRINT ''

SELECT 
    'user_info' AS TableName,
    COUNT(*) AS RecordCount
FROM user_info
UNION ALL
SELECT 'user_account', COUNT(*) FROM user_account
UNION ALL
SELECT 'user_auth', COUNT(*) FROM user_auth
UNION ALL
SELECT 'ssn', COUNT(*) FROM ssn

GO

-- =============================================
-- 6. VERIFY ACCOUNT BY EMAIL
-- =============================================
DECLARE @Email VARCHAR(50) = 'user@email.com'

PRINT ''
PRINT '========================================='
PRINT 'SEARCH BY EMAIL: ' + @Email
PRINT '========================================='
PRINT ''

SELECT 
    ui.account,
    s.email,
    s.mobile AS PIN,
    ua.uid,
    ui.create_date,
    ui.status_flag
FROM ssn s
INNER JOIN user_info ui ON s.ssn = ui.ssn
LEFT JOIN user_account ua ON ui.account = ua.account
WHERE s.email = @Email

GO

-- =============================================
-- 7. CHECK FOR DUPLICATE EMAILS
-- =============================================
PRINT ''
PRINT '========================================='
PRINT 'DUPLICATE EMAILS (Should be none)'
PRINT '========================================='
PRINT ''

SELECT 
    email,
    COUNT(*) AS DuplicateCount,
    STRING_AGG(name, ', ') AS Accounts
FROM ssn
WHERE email IS NOT NULL AND email <> ''
GROUP BY email
HAVING COUNT(*) > 1

GO

-- =============================================
-- 8. CHECK FOR DUPLICATE USERNAMES
-- =============================================
PRINT ''
PRINT '========================================='
PRINT 'DUPLICATE USERNAMES (Should be none)'
PRINT '========================================='
PRINT ''

SELECT 
    account,
    COUNT(*) AS DuplicateCount
FROM user_info
GROUP BY account
HAVING COUNT(*) > 1

GO

-- =============================================
-- 9. VERIFY PASSWORD FIELDS ARE POPULATED
-- =============================================
PRINT ''
PRINT '========================================='
PRINT 'ACCOUNTS WITH MISSING PASSWORDS'
PRINT '========================================='
PRINT ''

SELECT 
    account,
    CASE WHEN password IS NULL THEN 'MISSING' ELSE 'OK' END AS BinaryPassword,
    CASE WHEN passwd IS NULL THEN 'MISSING' ELSE 'OK' END AS WebPassword,
    CASE WHEN web_password IS NULL THEN 'MISSING' ELSE 'OK' END AS HexPassword
FROM user_auth
WHERE password IS NULL 
   OR passwd IS NULL 
   OR web_password IS NULL

GO

-- =============================================
-- 10. TEST ACCOUNT REGISTRATION SUMMARY
-- Quick overview of last 10 registrations
-- =============================================
PRINT ''
PRINT '========================================='
PRINT 'LAST 10 ACCOUNT REGISTRATIONS'
PRINT '========================================='
PRINT ''

SELECT TOP 10
    ui.account AS Username,
    s.email AS Email,
    s.mobile AS PIN,
    ui.create_date AS CreatedDate,
    CASE WHEN ua.uid IS NOT NULL THEN 'YES' ELSE 'NO' END AS HasUID,
    CASE WHEN uat.account IS NOT NULL THEN 'YES' ELSE 'NO' END AS HasAuth,
    CASE WHEN s.ssn IS NOT NULL THEN 'YES' ELSE 'NO' END AS HasSSN
FROM user_info ui
LEFT JOIN ssn s ON ui.ssn = s.ssn
LEFT JOIN user_account ua ON ui.account = ua.account
LEFT JOIN user_auth uat ON ui.account = uat.account
ORDER BY ui.create_date DESC

GO
