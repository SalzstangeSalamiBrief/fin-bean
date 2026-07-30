# Product

## fin-bean

A personal finance tracking app. Authentication and user management (register, update, delete) is handled via Keycloak. A user can only see and manipulate their own data. Single-user accounts only (no shared/family accounts).

## v1.0.0

### Users & Profile

- User accounts managed via Keycloak (add/update/delete)
- Each user selects a single currency in their profile settings; all their amounts (salary, expenses, goals) use that currency
- User can opt in to "Spending Feedback" in their profile and fill out a questionnaire to build a spending profile
- Deleting a user's Keycloak account cascades to delete all their app data (expenses, goals, spending profile) for GDPR compliance

### Salary

- User can set a single recurring monthly salary value
- Editable at any time; the current value is used for ongoing calculations
- Historical (already completed) months keep the salary value that was active at that time; only the current/future month uses the latest value

### Expense Tracking

- Manual expense entry with fields: amount, date, category
- Fixed predefined category list (not user-editable in v1.0.0): Housing, Groceries, Transport, Utilities, Entertainment, Health, Shopping, Other
- Expenses can be grouped (by category) and sorted (e.g. by date, amount)
- Monthly overview: expenses for the month, total spent, remaining budget (salary - expenses)
- Budget alert: user-configurable threshold; user chooses one of:
  - % of salary spent
  - fixed remaining-amount threshold
  - Alert is shown when the threshold is reached/exceeded
- Expenses can be edited and deleted after creation
- A "month" is defined as a calendar month (1st to last day)

### End-of-month report

- In-app view only in v1.0.0 (email delivery considered for a later version)
- Contains: list of expenses with metadata, remaining budget for the month, optional spending feedback
- Users can browse past months' expenses and reports, not just the current month

### Spending Feedback (questionnaire built now, AI mocked)

- Questionnaire to build a "spending profile" is implemented in v1.0.0
- AI-based analysis of the spending profile against the report is mocked in v1.0.0; a real locally-hosted AI model is planned for a later version

### Goals

- Overall savings goal: target amount + target date, progress tracked against accumulated remaining budget
- Category spending-limit goal: max spend per category per month, using the same fixed predefined category list as expenses
- Goals are compared against actual spending/savings and progress is shown to the user

### Lifetime Savings

- Each month's remaining budget (salary - expenses) resets at month start; it does not roll over into the next month's budget/alert calculation
- The surplus/deficit from every completed month is accumulated into a running lifetime total, shown to the user

### Appearance & Localization

- Dark theme only in v1.0.0 (no light mode, no theme toggle)
- Supported languages: English and German
- User selects their language in profile settings
- Localization covers translated UI text plus locale-aware formatting of dates, numbers, and currency
- Responsive design: built for both desktop and mobile, mobile-first approach
- Icons are used throughout the UI:
  - Desktop: icons paired with text labels, plus a hover title/tooltip
  - Mobile: icons only, no text labels

## Deferred to later versions

- Receipt scanning via camera + locally-hosted OCR to extract expense positions
- AI-based automatic expense classification
- Real AI-based spending feedback analysis (v1.0.0 ships a mocked output)
- Email delivery of the monthly report
