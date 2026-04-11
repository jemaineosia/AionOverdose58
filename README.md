# Aion Overdose 58

A dark fantasy fansite for **Aion 2**, built with **.NET 9 Blazor Web App (Auto interactivity mode)**, **Tailwind CSS**, and **SQL Server** via Entity Framework Core.

---

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Frontend | .NET 9 Blazor Web App (Auto — SSR + WASM) |
| Styling | Tailwind CSS (via CDN for dev; PostCSS for production) |
| Fonts | Google Fonts: Cinzel + Raleway |
| Database | SQL Server via Entity Framework Core 9 |
| ORM | EF Core with Code-First migrations & seed data |

---

## Solution Structure

```
AionOverdose58/
├── AionOverdose58.slnx                        # Solution file
├── AionOverdose58.Shared/                     # Shared models & DTOs
│   ├── Models/
│   │   ├── NewsArticle.cs
│   │   ├── WikiEntry.cs
│   │   ├── PlayerRanking.cs
│   │   └── UserAccount.cs
│   └── DTOs/
│       ├── NewsArticleDto.cs
│       ├── WikiEntryDto.cs
│       ├── PlayerRankingDto.cs
│       └── UserAccountDto.cs
├── AionOverdose58.Data/                       # EF Core + SQL Server
│   ├── AppDbContext.cs
│   └── Migrations/
├── AionOverdose58.Web/
│   ├── AionOverdose58.Web/                    # Blazor Server (SSR + Interactive Server)
│   │   ├── Components/
│   │   │   ├── App.razor                      # Root with Tailwind CDN + Google Fonts
│   │   │   ├── Routes.razor
│   │   │   ├── Layout/
│   │   │   │   ├── MainLayout.razor
│   │   │   │   ├── NavBar.razor               # Logo, nav links, Account dropdown
│   │   │   │   └── SocialBar.razor            # Facebook, YouTube, Discord
│   │   │   ├── Pages/
│   │   │   │   ├── Home.razor                 # Hero slider + Latest News
│   │   │   │   ├── News.razor                 # News grid with pagination
│   │   │   │   ├── NewsDetail.razor           # Single article view
│   │   │   │   ├── Ranking.razor              # Player rankings table
│   │   │   │   ├── NotFound.razor             # 404 page
│   │   │   │   └── Account/
│   │   │   │       ├── Login.razor
│   │   │   │       ├── Register.razor
│   │   │   │       └── ControlPanel.razor
│   │   │   └── Shared/
│   │   │       └── HeroSlider.razor           # Full-screen auto-sliding hero
│   │   ├── Services/
│   │   │   ├── NewsService.cs
│   │   │   └── RankingService.cs
│   │   ├── Program.cs
│   │   ├── appsettings.json
│   │   └── wwwroot/
│   │       └── app.css                        # Custom scrollbar + minimal global CSS
│   └── AionOverdose58.Web.Client/             # Blazor WASM (Auto interactivity client)
│       └── Program.cs
```

---

## Setup Instructions

### Prerequisites
- .NET 9 SDK
- SQL Server (local or Azure)
- Node.js (optional, for Tailwind PostCSS in production)

### 1. Clone & Restore
```bash
git clone <repo-url>
cd AionOverdose58
dotnet restore
```

### 2. Configure Database
Update the connection string in `AionOverdose58.Web/AionOverdose58.Web/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=AionOverdose58;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

### 3. Run Database Migrations
```bash
dotnet ef database update \
  --project AionOverdose58.Data/AionOverdose58.Data.csproj \
  --startup-project AionOverdose58.Web/AionOverdose58.Web/AionOverdose58.Web.csproj
```
This will create the database and seed sample data (3 news articles, 5 player rankings, 3 wiki entries).

### 4. Run the Application
```bash
cd AionOverdose58.Web/AionOverdose58.Web
dotnet run
```
Open `https://localhost:5001` or `http://localhost:5000`.

---

## Production: Tailwind CSS with PostCSS

The current setup uses the Tailwind CDN for development. For production, switch to a PostCSS build:

1. Install Node.js dependencies:
```bash
cd AionOverdose58.Web/AionOverdose58.Web
npm init -y
npm install -D tailwindcss @tailwindcss/typography postcss autoprefixer
npx tailwindcss init
```

2. Create `tailwind.config.js` with content paths pointing to your `.razor` files.

3. Create `postcss.config.js`.

4. Add a build script and remove the CDN script tag from `App.razor`.

---

## How to Swap Placeholder Images

Placeholder images (`https://picsum.photos/...`) are used in:
- `HeroSlider.razor` — replace with real Aion 2 artwork URLs
- Seed data in `AppDbContext.cs` — update `ImageUrl` fields with real CDN URLs

Example Aion 2 CDN URLs (NCSoft official assets):
```
https://aion2.plaync.com/...
```
Be mindful of NCSoft's IP/copyright when using official game assets.

---

## Design Theme

| Token | Value |
|-------|-------|
| `aion-dark` | `#080b14` |
| `aion-red` | `#8b1a1a` |
| `aion-crimson` | `#c0392b` |
| `aion-gold` | `#c9a84c` |
| `aion-teal` | `#1a4a4a` |
| Heading font | Cinzel (serif, Google Fonts) |
| Body font | Raleway (sans-serif, Google Fonts) |

---

## Features

- 🌑 **Dark gothic/fantasy design** with full-viewport background and Cinzel/Raleway fonts
- 🎠 **Hero Slider** — full-screen, auto-advancing with arrows and dot indicators
- 📰 **News** — paginated list and detail pages, DB-backed with seed data fallback
- 🏆 **Player Rankings** — gold/silver/bronze highlights, DB-backed with fallback
- 👤 **Account** — Login, Register, Control Panel pages (auth logic TBD)
- 🔗 **Social Bar** — Fixed Facebook, YouTube, Discord buttons
- 📱 **Responsive** — mobile hamburger menu, responsive grid layouts
