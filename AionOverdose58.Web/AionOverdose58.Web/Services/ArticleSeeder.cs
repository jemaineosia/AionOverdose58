using AionOverdose58.Data;
using AionOverdose58.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace AionOverdose58.Web.Services;

public static class ArticleSeeder
{
    public static async Task SeedArticlesAsync(IServiceProvider serviceProvider)
    {
        var dbFactory = serviceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
        await using var db = await dbFactory.CreateDbContextAsync();

        // Skip if already seeded (10+ articles present means a real seed already ran)
        if (await db.Articles.CountAsync() >= 10)
            return;

        var now = DateTime.UtcNow;

        var articles = new List<Article>
        {
            // ── ANNOUNCEMENTS ────────────────────────────────────────────────

            new Article
            {
                Category       = ArticleCategory.Announcement,
                Title          = "Welcome to Aion Overdose 58 — Server Launch!",
                PreviewContent = "The realm awakens. Aion Overdose 58 is officially live — register your Daeva and join the eternal conflict between Elyos and Asmodian.",
                Content        = @"<h2>The Realm Awakens</h2>
<p>After months of preparation, <strong>Aion Overdose 58</strong> is officially open to the public. Whether you choose the radiant wings of the <em>Elyos</em> or the dark power of the <em>Asmodians</em>, your legend begins today.</p>
<h3>Server Rates</h3>
<ul>
  <li>EXP (Monsters): <strong>5x</strong></li>
  <li>EXP (Quests): <strong>5x</strong></li>
  <li>Drop Rate: <strong>2.5x</strong></li>
  <li>Crafting / Gathering: <strong>3x</strong></li>
</ul>
<p>Download the client, register your account, and take flight. Glory awaits those who dare claim it.</p>",
                ImageUrl       = "https://images.unsplash.com/photo-1614294149010-950b698f72c0?w=800",
                PostedDate     = now.AddDays(-30),
                CreatedOn      = now.AddDays(-30),
                UpdatedOn      = now.AddDays(-30),
            },

            new Article
            {
                Category       = ArticleCategory.Announcement,
                Title          = "New Instance: Beshmundir Temple Heroic Mode",
                PreviewContent = "The ancient vaults of Beshmundir Temple have been unlocked in Heroic mode. Gather your legion and claim the treasures within — if you survive.",
                Content        = @"<h2>Beshmundir Temple — Heroic Mode Unlocked</h2>
<p>The <strong>Heroic Mode</strong> of Beshmundir Temple is now available for groups of 6 seasoned Daevas. The dungeon features reworked mechanics, increased loot tables, and a new exclusive boss — <em>Stormwing the Eternal</em>.</p>
<h3>Rewards</h3>
<ul>
  <li>Fabled-grade weapons with custom skin variants</li>
  <li>Exclusive title: <strong>Conqueror of the Tempest</strong></li>
  <li>Abyss Point bonus caches (x3 normal rate)</li>
</ul>
<p>Entrance is limited to players at or above level 55. Form your party and enter from the Abyss portal in Silentera Canyon.</p>",
                ImageUrl       = "https://images.unsplash.com/photo-1542751371-adc38448a05e?w=800",
                PostedDate     = now.AddDays(-25),
                CreatedOn      = now.AddDays(-25),
                UpdatedOn      = now.AddDays(-25),
            },

            new Article
            {
                Category       = ArticleCategory.Announcement,
                Title          = "Abyss Siege Schedule Updated — New Times for All Fortresses",
                PreviewContent = "Following community feedback, the Abyss siege schedule has been revised. Check the new times for Divine Fortress, Sulfur Fortress, and Roah Fortress.",
                Content        = @"<h2>Updated Siege Schedule</h2>
<p>We have revised fortress siege times to better serve players across different time zones.</p>
<table border='1' style='width:100%;border-collapse:collapse;'>
  <tr><th>Fortress</th><th>Day</th><th>Time (Server)</th></tr>
  <tr><td>Divine Fortress</td><td>Saturday</td><td>20:00</td></tr>
  <tr><td>Sulfur Fortress</td><td>Wednesday</td><td>20:00</td></tr>
  <tr><td>Roah Fortress</td><td>Friday</td><td>21:00</td></tr>
  <tr><td>Asteria Fortress</td><td>Sunday</td><td>19:00</td></tr>
</table>
<p>All times are server time (UTC-5). Teleport scrolls to Abyss entry points are available from the NPC <em>Kalius</em> in Sanctum/Pandaemonium.</p>",
                ImageUrl       = "https://images.unsplash.com/photo-1639762681485-074b7f938ba0?w=800",
                PostedDate     = now.AddDays(-22),
                CreatedOn      = now.AddDays(-22),
                UpdatedOn      = now.AddDays(-22),
            },

            new Article
            {
                Category       = ArticleCategory.Announcement,
                Title          = "Character Transfers Now Open — Move Between Factions",
                PreviewContent = "For a limited time, character faction transfers are available. Switch from Elyos to Asmodian (or vice versa) and keep your progression.",
                Content        = @"<h2>Faction Transfer Service — Limited Window</h2>
<p>By popular demand, we are opening <strong>faction transfers</strong> for a 14-day window. Transfer your Daeva to the opposing faction while retaining:</p>
<ul>
  <li>Character level and skills</li>
  <li>Equipment and inventory</li>
  <li>Legion rank (Legion membership will be reset)</li>
</ul>
<p>Abyss Points will be reset to 0 upon transfer. To initiate a transfer, visit your <strong>Control Panel</strong> and select <em>Character Services</em>.</p>
<p><strong>Transfer window closes in 14 days.</strong></p>",
                ImageUrl       = "https://images.unsplash.com/photo-1511512578047-dfb367046420?w=800",
                PostedDate     = now.AddDays(-18),
                CreatedOn      = now.AddDays(-18),
                UpdatedOn      = now.AddDays(-18),
            },

            new Article
            {
                Category       = ArticleCategory.Announcement,
                Title          = "Anti-Cheat System Upgraded — Zero Tolerance Policy",
                PreviewContent = "We have deployed a new anti-cheat layer. Players found using bots, speed hacks, or exploits will be permanently banned without appeal.",
                Content        = @"<h2>Integrity of the Realm</h2>
<p>Aion Overdose 58 is committed to a fair and competitive environment. We have upgraded our detection systems with a new behavioral analysis layer that operates in real-time.</p>
<h3>Bannable Offenses</h3>
<ul>
  <li>Automated bots or macro scripts</li>
  <li>Speed, flight, or teleport hacks</li>
  <li>Exploit abuse (must be reported, not used)</li>
  <li>Account sharing in PvP contexts</li>
</ul>
<p>Reports can be submitted via Discord or the in-game ticket system. Players who report valid exploits will be rewarded with <strong>1,000 AP Bonus Caches</strong>.</p>",
                ImageUrl       = "https://images.unsplash.com/photo-1550751827-4bd374c3f58b?w=800",
                PostedDate     = now.AddDays(-15),
                CreatedOn      = now.AddDays(-15),
                UpdatedOn      = now.AddDays(-15),
            },

            new Article
            {
                Category       = ArticleCategory.Announcement,
                Title          = "Server Maintenance — April 22 from 02:00 to 06:00 Server Time",
                PreviewContent = "Scheduled maintenance on April 22. The server will be offline from 02:00 to 06:00. All online players will receive a 4-hour EXP boost after the restart.",
                Content        = @"<h2>Scheduled Maintenance Notice</h2>
<p>We will be performing scheduled maintenance on <strong>April 22</strong> from <strong>02:00 to 06:00 server time</strong>.</p>
<h3>What's happening during maintenance</h3>
<ul>
  <li>Database optimization and backup</li>
  <li>Deployment of patch 1.2.1 (see Patch Notes)</li>
  <li>Instance reset cycle synchronization</li>
</ul>
<h3>Compensation</h3>
<p>All accounts active in the last 7 days will receive a <strong>4-hour 100% EXP Boost</strong> automatically applied after the server restart. No action required.</p>",
                ImageUrl       = "https://images.unsplash.com/photo-1558494949-ef010cbdcc31?w=800",
                PostedDate     = now.AddDays(-5),
                CreatedOn      = now.AddDays(-5),
                UpdatedOn      = now.AddDays(-5),
            },

            // ── PATCH NOTES ──────────────────────────────────────────────────

            new Article
            {
                Category       = ArticleCategory.PatchNotes,
                Title          = "Patch 1.0 — Launch Patch Notes",
                PreviewContent = "Full notes for the launch patch: class rebalancing, Abyss economy tweaks, and quality-of-life improvements for all 11 classes.",
                Content        = @"<h2>Patch 1.0 — Launch</h2>
<h3>Class Balancing</h3>
<ul>
  <li><strong>Gladiator:</strong> Iron Skin cooldown reduced from 3 min → 2.5 min.</li>
  <li><strong>Sorcerer:</strong> Flame Cage damage increased by 8%.</li>
  <li><strong>Chanter:</strong> Word of Healing base coefficient increased by 5%.</li>
  <li><strong>Assassin:</strong> Sigil Strike critical multiplier reduced from 2.3x → 2.1x (PvP only).</li>
  <li><strong>Ranger:</strong> Focused Shots cast time reduced by 0.2s.</li>
</ul>
<h3>Economy</h3>
<ul>
  <li>Kinah drop from elites increased by 15%.</li>
  <li>Broker fees reduced from 10% → 7%.</li>
</ul>
<h3>Quality of Life</h3>
<ul>
  <li>Auto-loot toggle added to settings menu.</li>
  <li>Quest tracker now supports 25 active quests (up from 10).</li>
</ul>",
                ImageUrl       = "https://images.unsplash.com/photo-1461749280684-dccba630e2f6?w=800",
                PostedDate     = now.AddDays(-29),
                CreatedOn      = now.AddDays(-29),
                UpdatedOn      = now.AddDays(-29),
            },

            new Article
            {
                Category       = ArticleCategory.PatchNotes,
                Title          = "Patch 1.1 — Abyss & PvP Overhaul",
                PreviewContent = "Major changes to Abyss Point scaling, siege mechanics, and PvP gear progression. Templar and Cleric receive notable skill adjustments.",
                Content        = @"<h2>Patch 1.1 — Abyss & PvP Overhaul</h2>
<h3>Abyss Changes</h3>
<ul>
  <li>AP rewards from solo kills scaled down by 10% to reduce kill-trading.</li>
  <li>Fortress siege rewards now scale with contribution score rather than kill count.</li>
  <li>New Abyss rank: <strong>Supreme Commander</strong> — requires 15,000,000 AP.</li>
</ul>
<h3>PvP Gear</h3>
<ul>
  <li>Governor-grade weapons now have a secondary enchantment slot.</li>
  <li>PvP Defense rating on Legion Guardian gear increased by 12%.</li>
</ul>
<h3>Class Adjustments</h3>
<ul>
  <li><strong>Templar:</strong> Divine Curtain HP absorption cap raised from 3,000 → 4,500.</li>
  <li><strong>Cleric:</strong> Healing Light cast time reduced from 2.5s → 2.0s in PvP.</li>
  <li><strong>Spiritmaster:</strong> Spirit's Empowerment buff now persists through death.</li>
</ul>",
                ImageUrl       = "https://images.unsplash.com/photo-1538481199705-c710c4e965fc?w=800",
                PostedDate     = now.AddDays(-20),
                CreatedOn      = now.AddDays(-20),
                UpdatedOn      = now.AddDays(-20),
            },

            new Article
            {
                Category       = ArticleCategory.PatchNotes,
                Title          = "Patch 1.1.5 — Instance Tuning & Bug Fixes",
                PreviewContent = "Hot-fix patch addressing critical bugs in Dark Poeta, Udas Temple, and several skill interactions reported by the community.",
                Content        = @"<h2>Patch 1.1.5 — Hotfix</h2>
<h3>Instance Fixes</h3>
<ul>
  <li><strong>Dark Poeta:</strong> Fixed Tahabata Pyrestaff resetting to full HP when the raid wiped during Phase 2.</li>
  <li><strong>Udas Temple:</strong> Corrected loot table — Archon Siege Weapon Fragment was missing from final boss.</li>
  <li><strong>Steel Rake:</strong> Fixed an issue where the ship's engine room door would not open after defeating Vile Judge Kromede.</li>
</ul>
<h3>Skill Fixes</h3>
<ul>
  <li>Gladiator <em>Ferocious Strike</em> no longer applies a duplicate bleed stack.</li>
  <li>Ranger <em>Ankle Snare</em> correctly applies slow instead of root in open-world PvP.</li>
  <li>Fixed a visual bug where Sorcerer ice effects persisted after death.</li>
</ul>
<h3>Miscellaneous</h3>
<ul>
  <li>Legion storage capacity increased from 80 → 120 slots.</li>
  <li>Fixed sorting in the broker search by price per unit.</li>
</ul>",
                ImageUrl       = "https://images.unsplash.com/photo-1555949963-ff9fe0c870eb?w=800",
                PostedDate     = now.AddDays(-14),
                CreatedOn      = now.AddDays(-14),
                UpdatedOn      = now.AddDays(-14),
            },

            new Article
            {
                Category       = ArticleCategory.PatchNotes,
                Title          = "Patch 1.2 — Crafting System Rework",
                PreviewContent = "A complete overhaul of the crafting system: new recipes, reduced material costs, and the introduction of the Masterwork system.",
                Content        = @"<h2>Patch 1.2 — Crafting Rework</h2>
<h3>Masterwork System</h3>
<p>Crafted items now have a chance to become <strong>Masterwork</strong> quality, granting a +10% stat bonus over the base version. The chance scales with your crafting skill level:</p>
<ul>
  <li>Skill 400–449: 5% Masterwork chance</li>
  <li>Skill 450–499: 10% Masterwork chance</li>
  <li>Skill 500 (Expert): 15% Masterwork chance</li>
</ul>
<h3>Recipe Changes</h3>
<ul>
  <li>Fabled armor recipe material cost reduced by 20%.</li>
  <li>New recipes added: <strong>Daevanion Essences</strong> craftable at Expert level.</li>
  <li>Alchemy now produces usable combat potions with new stat profiles.</li>
</ul>
<h3>Quality of Life</h3>
<ul>
  <li>Craft queue can now hold up to 50 items.</li>
  <li>Added a material-gathering helper that highlights nearby nodes on the minimap.</li>
</ul>",
                ImageUrl       = "https://images.unsplash.com/photo-1606144042614-b2417e99c4e3?w=800",
                PostedDate     = now.AddDays(-10),
                CreatedOn      = now.AddDays(-10),
                UpdatedOn      = now.AddDays(-10),
            },

            new Article
            {
                Category       = ArticleCategory.PatchNotes,
                Title          = "Patch 1.2.1 — Balance Pass & Performance Update",
                PreviewContent = "Minor balance tweaks for Ranger and Gunslinger, plus a major server-side performance improvement reducing lag during large sieges.",
                Content        = @"<h2>Patch 1.2.1</h2>
<h3>Balance</h3>
<ul>
  <li><strong>Ranger:</strong> Aimed Shot damage increased by 5% in PvE only.</li>
  <li><strong>Gunslinger:</strong> Burst Fire recoil animation reduced, enabling faster combo transitions.</li>
  <li><strong>Aethertech:</strong> Mech armor passive defense increased from 8% → 11%.</li>
</ul>
<h3>Performance</h3>
<ul>
  <li>Siege instance server optimized — expected 30% reduction in skill-cast lag during 100+ player encounters.</li>
  <li>Reduced memory footprint of the Abyss map by streamlining NPC spawn data.</li>
</ul>
<h3>Bug Fixes</h3>
<ul>
  <li>Fixed Gunner Suppressive Fire not applying the defense debuff in PvP.</li>
  <li>Corrected an issue causing disconnects when looting chests near instance exits.</li>
</ul>",
                ImageUrl       = "https://images.unsplash.com/photo-1518770660439-4636190af475?w=800",
                PostedDate     = now.AddDays(-3),
                CreatedOn      = now.AddDays(-3),
                UpdatedOn      = now.AddDays(-3),
            },

            // ── EVENTS ───────────────────────────────────────────────────────

            new Article
            {
                Category       = ArticleCategory.Event,
                Title          = "Double EXP Weekend — April 19–21",
                PreviewContent = "Celebrate the server's first month with a Double EXP weekend. All EXP gains are doubled from Friday 18:00 to Sunday midnight.",
                Content        = @"<h2>Double EXP Weekend 🎉</h2>
<p>To celebrate Aion Overdose 58's first month, we are hosting a <strong>Double EXP Weekend</strong>!</p>
<h3>Event Details</h3>
<ul>
  <li><strong>Start:</strong> Friday, April 19 — 18:00 server time</li>
  <li><strong>End:</strong> Sunday, April 21 — 23:59 server time</li>
  <li><strong>Bonus:</strong> 2x EXP from all sources (monsters, quests, crafting)</li>
</ul>
<p>This is the perfect opportunity to push past level 40 or grind your crafting skill to Expert. Get online and take flight!</p>",
                ImageUrl       = "https://images.unsplash.com/photo-1492684223066-81342ee5ff30?w=800",
                PostedDate     = now.AddDays(-7),
                CreatedOn      = now.AddDays(-7),
                UpdatedOn      = now.AddDays(-7),
            },

            new Article
            {
                Category       = ArticleCategory.Event,
                Title          = "Dredgion PvP Tournament — Register Your Team",
                PreviewContent = "A 6v6 Dredgion PvP tournament is coming. Register your team for a chance to win a full set of Governor-grade gear and the exclusive Legion Banner of Champions.",
                Content        = @"<h2>Dredgion PvP Tournament</h2>
<p>Think your legion is the strongest on the server? Prove it in our <strong>6v6 Dredgion Tournament</strong>.</p>
<h3>Format</h3>
<ul>
  <li>Single elimination bracket — 16 teams maximum</li>
  <li>Standard Dredgion ruleset (no external consumables)</li>
  <li>Matches streamed on our Discord</li>
</ul>
<h3>Prizes</h3>
<ul>
  <li>🥇 <strong>1st Place:</strong> Full Governor-grade gear set + Legion Banner of Champions</li>
  <li>🥈 <strong>2nd Place:</strong> Fabled weapon of choice + 500,000 AP each</li>
  <li>🥉 <strong>3rd Place:</strong> 250,000 AP each + enchantment stone caches</li>
</ul>
<p>Registration closes when 16 teams have signed up. Register via the Discord #tournament channel.</p>",
                ImageUrl       = "https://images.unsplash.com/photo-1560253023-3ec5d502959f?w=800",
                PostedDate     = now.AddDays(-12),
                CreatedOn      = now.AddDays(-12),
                UpdatedOn      = now.AddDays(-12),
            },

            new Article
            {
                Category       = ArticleCategory.Event,
                Title          = "World Boss: Tiamat Descends — Worldwide PvPvE Event",
                PreviewContent = "Tiamat herself manifests in the open world. Both factions must cooperate — and compete — to deal the killing blow and claim the Dragonlord's hoard.",
                Content        = @"<h2>Tiamat Descends — World Event</h2>
<p>For the first time on Aion Overdose 58, <strong>Tiamat the Dragonlord</strong> will appear in the open world. Both Elyos and Asmodian players can attack her — but so can each other.</p>
<h3>Event Rules</h3>
<ul>
  <li>PvP is enabled in the entire event zone for the duration</li>
  <li>The faction dealing the killing blow receives double loot</li>
  <li>All participants receive a participation reward based on damage contribution</li>
</ul>
<h3>Loot Pool</h3>
<ul>
  <li>Tiamat's Scale (crafting material for Dragonlord weapons)</li>
  <li>Ancient Manastone Bundles</li>
  <li>Draconic Title: <strong>Dragonslayer</strong></li>
</ul>
<p>Event date: <strong>April 26 at 20:00 server time.</strong> Location will be revealed 1 hour before the event.</p>",
                ImageUrl       = "https://images.unsplash.com/photo-1570641963303-92ce4845ed4c?w=800",
                PostedDate     = now.AddDays(-9),
                CreatedOn      = now.AddDays(-9),
                UpdatedOn      = now.AddDays(-9),
            },

            new Article
            {
                Category       = ArticleCategory.Event,
                Title          = "Harvest Festival — Collect Lunacherry Tokens for Exclusive Rewards",
                PreviewContent = "The Harvest Festival is here. Farm Lunacherry Tokens from seasonal mobs and trade them for costumes, mounts, and housing items.",
                Content        = @"<h2>Harvest Festival 🍂</h2>
<p>The <strong>Harvest Festival</strong> runs from <strong>April 20 to May 5</strong>. Seasonal mobs have spawned across all zones and drop <em>Lunacherry Tokens</em>.</p>
<h3>Reward Exchange</h3>
<table border='1' style='width:100%;border-collapse:collapse;'>
  <tr><th>Item</th><th>Cost (Lunacherry Tokens)</th></tr>
  <tr><td>Harvest Festival Costume Set</td><td>200</td></tr>
  <tr><td>Striped Porgus Mount</td><td>500</td></tr>
  <tr><td>Luna Lantern Housing Item</td><td>50</td></tr>
  <tr><td>EXP Boost (2 hours)</td><td>30</td></tr>
  <tr><td>Fabled Manastone Bundle</td><td>150</td></tr>
</table>
<p>Exchange NPC <strong>Festival Quartermaster Eryn</strong> can be found in Sanctum and Pandaemonium.</p>",
                ImageUrl       = "https://images.unsplash.com/photo-1508193638397-1c4234db14d8?w=800",
                PostedDate     = now.AddDays(-6),
                CreatedOn      = now.AddDays(-6),
                UpdatedOn      = now.AddDays(-6),
            },

            new Article
            {
                Category       = ArticleCategory.Event,
                Title          = "Legion War Season 1 — Rankings & Glory Points Launch",
                PreviewContent = "Season 1 of the Legion War begins. Legions earn Glory Points through sieges, PvP kills, and world events. Top 3 legions win exclusive rewards at season end.",
                Content        = @"<h2>Legion War — Season 1</h2>
<p><strong>Season 1</strong> of the Aion Overdose 58 Legion War begins today and runs until <strong>May 31</strong>. Compete for server dominance and eternal glory.</p>
<h3>How to Earn Glory Points</h3>
<ul>
  <li>Successful fortress siege: <strong>1,000 GP</strong></li>
  <li>Defeating an enemy officer (rank 3+): <strong>50 GP</strong></li>
  <li>World boss contribution: <strong>up to 500 GP</strong></li>
  <li>Winning Dredgion as a full-legion premade: <strong>100 GP</strong></li>
</ul>
<h3>Season End Rewards</h3>
<ul>
  <li>🥇 <strong>1st Legion:</strong> Exclusive Legion Hall skin + 1,000 Gold Ingots per member</li>
  <li>🥈 <strong>2nd Legion:</strong> 500 Gold Ingots per member + fabled accessories</li>
  <li>🥉 <strong>3rd Legion:</strong> 250 Gold Ingots per member</li>
</ul>",
                ImageUrl       = "https://images.unsplash.com/photo-1579373903781-fd5c0c30c4cd?w=800",
                PostedDate     = now.AddDays(-4),
                CreatedOn      = now.AddDays(-4),
                UpdatedOn      = now.AddDays(-4),
            },

            new Article
            {
                Category       = ArticleCategory.Event,
                Title          = "Ascension Day Celebration — Log In for 7 Days of Gifts",
                PreviewContent = "Commemorate the day the Daevas ascended. Log in each day for 7 consecutive days and claim increasingly powerful rewards.",
                Content        = @"<h2>Ascension Day Celebration 🌟</h2>
<p>In honor of Daeva Ascension Day, we are distributing a <strong>7-day login reward calendar</strong>. Log in each day to claim your gift.</p>
<h3>Reward Calendar</h3>
<table border='1' style='width:100%;border-collapse:collapse;'>
  <tr><th>Day</th><th>Reward</th></tr>
  <tr><td>Day 1</td><td>EXP Boost x2 (1 hour each)</td></tr>
  <tr><td>Day 2</td><td>100,000 Kinah</td></tr>
  <tr><td>Day 3</td><td>Greater Supplements Bundle (x10)</td></tr>
  <tr><td>Day 4</td><td>Blessed Enchantment Stone (x5)</td></tr>
  <tr><td>Day 5</td><td>Abyss Point Cache (50,000 AP)</td></tr>
  <tr><td>Day 6</td><td>Fabled Manastone Bundle</td></tr>
  <tr><td>Day 7</td><td>Ascension Wings (7-day cosmetic) + 500,000 Kinah</td></tr>
</table>
<p>Rewards are automatically sent to your in-game mailbox upon login each day.</p>",
                ImageUrl       = "https://images.unsplash.com/photo-1483728642387-6c3bdd6c93e5?w=800",
                PostedDate     = now.AddDays(-2),
                CreatedOn      = now.AddDays(-2),
                UpdatedOn      = now.AddDays(-2),
            },

            // ── SALES ────────────────────────────────────────────────────────

            new Article
            {
                Category       = ArticleCategory.Sales,
                Title          = "VIP Membership — Enhanced Rates & Exclusive Benefits",
                PreviewContent = "Become a VIP Daeva and unlock enhanced drop rates, priority queue access, exclusive cosmetics, and a monthly Gold Ingot stipend.",
                Content        = @"<h2>VIP Membership</h2>
<p>Support the server and unlock premium benefits with a <strong>VIP Membership</strong>.</p>
<h3>VIP Benefits</h3>
<ul>
  <li>+50% EXP boost (stacks with events)</li>
  <li>+25% Drop rate bonus</li>
  <li>Priority login queue (no waiting on full server)</li>
  <li>Exclusive VIP-only cosmetic skins each month</li>
  <li>Monthly stipend: <strong>50 Gold Ingots</strong></li>
  <li>Access to VIP lounge in Sanctum/Pandaemonium</li>
  <li>Custom <em>Daeva of Light/Shadow</em> title</li>
</ul>
<p>VIP Membership is available in 30-day, 90-day, and lifetime tiers. All purchases go directly toward server infrastructure and development.</p>",
                ImageUrl       = "https://images.unsplash.com/photo-1526374965328-7f61d4dc18c5?w=800",
                PostedDate     = now.AddDays(-28),
                CreatedOn      = now.AddDays(-28),
                UpdatedOn      = now.AddDays(-28),
            },

            new Article
            {
                Category       = ArticleCategory.Sales,
                Title          = "Cosmetic Shop Update — Spring Wings & Dye Bundles",
                PreviewContent = "New cosmetic items have arrived in the Gold Ingot Shop: Spring Petal Wings, Pastel Dye Set, and the limited Sakura Hanbok costume.",
                Content        = @"<h2>Cosmetic Shop — Spring Update</h2>
<p>New items are now available in the <strong>Gold Ingot Shop</strong> (accessible via the in-game shop NPC):</p>
<h3>New Arrivals</h3>
<ul>
  <li>🌸 <strong>Spring Petal Wings</strong> — 200 Gold Ingots (permanent)</li>
  <li>🎨 <strong>Pastel Dye Set</strong> (12 colors) — 80 Gold Ingots</li>
  <li>👘 <strong>Sakura Hanbok Costume</strong> — 350 Gold Ingots (limited, 30 days remaining)</li>
  <li>🐇 <strong>White Rabbit Housing Pet</strong> — 120 Gold Ingots</li>
</ul>
<h3>Returning Items</h3>
<ul>
  <li>Dark Abyss Wings — 180 Gold Ingots</li>
  <li>Blood Knight Armor Skin Set — 300 Gold Ingots</li>
</ul>
<p>All cosmetics are purely aesthetic and provide no gameplay advantage.</p>",
                ImageUrl       = "https://images.unsplash.com/photo-1490750967868-88df5691cc5a?w=800",
                PostedDate     = now.AddDays(-16),
                CreatedOn      = now.AddDays(-16),
                UpdatedOn      = now.AddDays(-16),
            },

            new Article
            {
                Category       = ArticleCategory.Sales,
                Title          = "Gold Ingot Bundle Sale — 30% Bonus This Weekend",
                PreviewContent = "Top up your Gold Ingot balance this weekend and receive 30% extra ingots on every purchase tier. Limited 72-hour window.",
                Content        = @"<h2>Gold Ingot Bonus Sale 🏅</h2>
<p>For the next <strong>72 hours</strong>, every Gold Ingot purchase comes with a <strong>30% bonus</strong>.</p>
<h3>Bonus Table</h3>
<table border='1' style='width:100%;border-collapse:collapse;'>
  <tr><th>Package</th><th>Base Ingots</th><th>Bonus</th><th>Total</th></tr>
  <tr><td>Starter</td><td>100</td><td>+30</td><td>130</td></tr>
  <tr><td>Adventurer</td><td>300</td><td>+90</td><td>390</td></tr>
  <tr><td>Champion</td><td>700</td><td>+210</td><td>910</td></tr>
  <tr><td>Daeva Lord</td><td>1,500</td><td>+450</td><td>1,950</td></tr>
</table>
<p>All purchases are processed immediately via the payment portal in your Control Panel. Gold Ingots never expire and are account-bound.</p>",
                ImageUrl       = "https://images.unsplash.com/photo-1611974789855-9c2a0a7236a3?w=800",
                PostedDate     = now.AddDays(-8),
                CreatedOn      = now.AddDays(-8),
                UpdatedOn      = now.AddDays(-8),
            },

            new Article
            {
                Category       = ArticleCategory.Sales,
                Title          = "Mount Bundle — Fenris Wolf & Stormwing Drake Now Available",
                PreviewContent = "Two legendary mounts have arrived in the Gold Ingot Shop: the Fenris Wolf mount for ground travel and the Stormwing Drake for aerial combat.",
                Content        = @"<h2>New Mounts — Fenris Wolf & Stormwing Drake</h2>
<p>Two legendary mounts are now available permanently in the Gold Ingot Shop:</p>
<h3>Fenris Wolf</h3>
<ul>
  <li>Ground mount — 160% movement speed</li>
  <li>Unique howl emote on interaction</li>
  <li>Cost: <strong>400 Gold Ingots</strong></li>
</ul>
<h3>Stormwing Drake</h3>
<ul>
  <li>Flying mount — 200% flight speed</li>
  <li>Lightning-trail visual effect during flight</li>
  <li>Counts as a war dragon for certain quest lines</li>
  <li>Cost: <strong>600 Gold Ingots</strong></li>
</ul>
<h3>Bundle Deal</h3>
<p>Purchase both mounts together for <strong>900 Gold Ingots</strong> (save 100) via the <em>Legendary Rider Bundle</em> in the shop.</p>",
                ImageUrl       = "https://images.unsplash.com/photo-1504194921103-f8b80cadd5e4?w=800",
                PostedDate     = now.AddDays(-1),
                CreatedOn      = now.AddDays(-1),
                UpdatedOn      = now.AddDays(-1),
            },
        };

        await db.Articles.AddRangeAsync(articles);
        await db.SaveChangesAsync();
    }
}
