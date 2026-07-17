const milestones = [
  ["Player movement", "complete"], ["Camera system", "complete"],
  ["Shardling chase", "complete"], ["Combat", "complete"],
  ["Health system", "complete"], ["Death feedback", "in progress"],
  ["Gate objective", "next"],
];

const saga = [
  ["Beneath Eden", "Curiosity", "Elijah Blue begins experiencing dreams that are not entirely his own and discovers the first traces of AZEL beneath West Ridge."],
  ["The Guardian’s Dream", "Trust", "Elijah’s dream-entanglement with AZEL deepens, and he must decide whether the presence inside his mind is guiding him—or using him."],
  ["The Garden Below", "Discovery", "Elijah enters the buried pre-Edenic world and learns that AZEL is living architecture extending through an entire hidden realm."],
  ["The Shattered Architecture", "Identity", "The boundary between Elijah Blue and Elijah Blaze becomes increasingly uncertain as damaged portions of the ancient system awaken."],
  ["The Seven Echoes", "Sacrifice", "Seven surviving echoes of the old world reveal competing pieces of the truth, each demanding something from Elijah."],
  ["The Last Kavanah", "Choice", "Elijah’s bond with AZEL reaches its greatest strength, but righteousness—not power—determines whether the architecture will obey him."],
  ["The Flame Above Eden", "Restoration", "The hidden conflict rises into the visible world, and Elijah must restore what was broken without becoming the guardian the ancient world feared."],
];

const comics = [
  ["Hollow Court", "Opening arc", "Elijah enters a damaged court of living architecture and encounters the first Shardlings."],
  ["The First Guardian", "AZEL prequel", "Fragments of the ancient guardian’s history—without fully revealing the pre-Edenic civilization."],
  ["West Ridge", "Grounded mystery", "Disappearances, symbols, survey records, and disturbances beneath the Arizona desert."],
  ["Shardfall", "Limited series", "The first major breach of the Qlippot into the surface world."],
  ["The Seven Echoes", "Companion anthology", "Each issue follows an ancient echo, lost chamber, guardian remnant, or moral failure."],
  ["Nora", "Side-story series", "Elijah’s transformation seen from outside his own mind—and partly through Nora’s eyes."],
  ["Before the Flame", "Later prequel", "Family, curiosity, diary, school life, and Elijah Blue’s earliest dreams before full synchronization."],
];

const codexEntries = [
  ["01", "Elijah Blue", "The dreamer", "A sixteen-year-old searching for the line between memory and inheritance."],
  ["02", "AZEL", "Living architecture", "An ancient intelligence sleeping beneath the Arizona desert."],
  ["03", "Hollow Court", "Threshold site", "A ceremonial ruin where the architecture first begins to answer."],
  ["04", "Shardlings", "Qlippotic fragments", "Hungry pieces of a broken pattern. Fast, watchful, unfinished."],
  ["05", "Kavanah", "Directed intent", "Purpose made active—the discipline that gives power its shape."],
  ["06", "Synchronization", "Archive sealed", "Identity, memory, and architecture entering dangerous resonance."],
];

export default function Home() {
  return (
    <main>
      <header className="nav-shell">
        <a className="brand" href="#home" aria-label="Elijah Blaze home"><span className="brand-mark">EB</span><span>ELIJAH BLAZE</span></a>
        <nav aria-label="Primary navigation">
          <a href="#game">Game</a><a href="#books">Beneath Eden</a><a href="#codex">Codex</a><a href="#studio">Studio</a><a href="#signal">Signal</a>
        </nav>
        <a className="nav-signal" href="#signal"><span />SIGNAL LIVE</a>
      </header>

      <section className="hero" id="home">
        <div className="hero-atmosphere" aria-hidden="true"><div className="sun"/><div className="monolith"/><div className="signal-line"/></div>
        <div className="avatar-frame">
          <img src="/assets/elijah-blaze-avatar.png" alt="Elijah Blaze in blue-and-gold living guardian armor, crowned by flame" />
          <div className="avatar-caption"><span>GUARDIAN FORM</span><b>ELIJAH + AZEL</b></div>
        </div>
        <div className="eyebrow"><span>PROJECT AZEL</span><span className="line"/><span>TRANSMISSION 001</span></div>
        <h1>ELIJAH<br/><em>BLAZE</em></h1>
        <p className="hero-tag">Some dreams are not your own.</p>
        <p className="hero-copy">Elijah Blue is sixteen, curious, and increasingly haunted by dreams of a place that should not exist. Beneath the Arizona desert waits AZEL—an ancient living architecture whose sleeping mind is slowly becoming entangled with his own.</p>
        <div className="hero-actions"><a className="button primary" href="#books">Enter Beneath Eden <span>↗</span></a><a className="button text-button" href="#game">Follow development <span>↓</span></a></div>
        <div className="hero-index"><span>16-YEAR-OLD DREAMER</span><span>ARIZONA / EARTH</span><span>SYNC STATUS: <b>EMERGING</b></span></div>
      </section>

      <section className="game section" id="game">
        <div className="section-heading"><div><p className="kicker">01 / THE GAME</p><h2>THE HOLLOW<br/><i>COURT</i></h2></div><p className="lead">A third-person action prototype about intention, resonance, and surviving the first place that dreams back.</p></div>
        <div className="game-grid">
          <div className="prototype-visual"><div className="reticle">◇</div><div className="hud"><span>PROTOTYPE // UNITY</span><b>HOLLOW COURT</b></div><div className="coordinates">33.4484° N<br/>112.0740° W</div></div>
          <div className="milestone-card">
            <div className="card-top"><p>CURRENT MILESTONE</p><span>BUILD 0.1.7</span></div><h3>Hollow Court Prototype</h3><div className="progress"><span/></div><p className="progress-label">5 OF 7 SYSTEMS ONLINE <b>71%</b></p>
            <div className="milestones">{milestones.map(([name,status],i)=><div className="milestone" key={name}><span className={`status ${status.replace(' ','-')}`}>{status === 'complete' ? '✓' : status === 'in progress' ? '◒' : '○'}</span><span>{name}</span><small>{String(i+1).padStart(2,'0')} / 07</small></div>)}</div>
          </div>
        </div>
        <div className="system-strip"><article><span>COMBAT SYSTEM</span><h3>Intent becomes impact.</h3><p>Close-range encounters where timing and purpose shape the exchange.</p></article><article><span>KAVANAH</span><h3>Power needs direction.</h3><p>A synchronization system built around focus—not accumulation.</p></article><article><span>SHARDLINGS</span><h3>The fragments are hungry.</h3><p>Reactive enemies born from patterns AZEL could not contain.</p></article></div>
      </section>

      <section className="books section" id="books">
        <div className="book-art"><div className="book-cover"><p>A NOVEL</p><div className="glyph">◈</div><h3>BENEATH<br/><em>EDEN</em></h3><small>BOOK ONE OF THE ELIJAH BLAZE SAGA</small></div><div className="book-shadow"/></div>
        <div className="book-copy"><p className="kicker">02 / THE BOOKS</p><h2>Before Elijah Blaze,<br/>there was <i>Elijah Blue.</i></h2><blockquote>“I used to think dreams were the one place nobody else could follow you.”</blockquote><p>Book One begins a seven-part saga of discovery, moral courage, and an impossible relationship between a boy and the intelligence sleeping below his world.</p><div className="book-meta"><span><small>FORMAT</small>NOVEL</span><span><small>SERIES</small>01 / 07</span><span><small>STATUS</small>IN DEVELOPMENT</span></div><a className="button primary" href="#signal">Read the first transmission <span>↗</span></a></div>
        <div className="saga-roadmap"><p>THE SEVEN-BOOK SAGA</p>{saga.map(([title,theme,description],i)=><article className={i===0?'active':''} key={title}><span>0{i+1}</span><div><b>{title}</b><p>{description}</p></div><small>{theme}</small></article>)}</div>
        <div className="comics-intro"><div><p className="kicker">ELIJAH BLAZE COMICS</p><h2>Beside. Before.<br/><i>Beneath.</i></h2></div><blockquote>The novels tell Elijah’s central journey. The comics reveal stories happening beside, before, and beneath it.</blockquote></div>
        <div className="comics-grid">{comics.map(([title,format,description],i)=><article key={title}><span>COMIC FILE / 0{i+1}</span><p>{format}</p><h3>Elijah Blaze:<br/><i>{title}</i></h3><div className="comic-sigil">{['◇','◉','⌖','✦','⑦','○','△'][i]}</div><p className="comic-copy">{description}</p><small>{i===0?'OPENING ARC':'DEVELOPMENT SLATE'}</small></article>)}</div>
      </section>

      <section className="codex section" id="codex">
        <div className="section-heading"><div><p className="kicker">03 / BLAZE CODEX</p><h2>THE ARCHIVE<br/><i>REMEMBERS.</i></h2></div><p className="lead">Recovered people, places, systems, and symbols from inside the Elijah Blaze universe. Some knowledge remains deliberately sealed.</p></div>
        <div className="codex-grid">{codexEntries.map(([n,title,type,copy],i)=><article className={i===5?'sealed':''} key={title}><span className="entry-number">{n}</span><p>{type}</p><h3>{title}</h3><div className="entry-glyph">{['◉','⬡','△','✦','⌁','◈'][i]}</div><p className="entry-copy">{copy}</p><button aria-label={`Open ${title} Codex entry`}>{i===5?'SEALED':'OPEN FILE'} <span>{i===5?'⌁':'↗'}</span></button></article>)}</div>
        <div className="archive-lock"><span>◈</span><div><p>RESTRICTED STRATUM</p><h3>ARCHIVE SEALED — SYNCHRONIZATION REQUIRED</h3></div><small>ACCESS LEVEL 07</small></div>
      </section>

      <section className="studio section" id="studio">
        <div className="studio-copy"><p className="kicker">04 / THE STUDIO</p><h2>WORLDS WITH<br/><i>PURPOSE.</i></h2><p className="manifesto">We create worlds built around wonder, moral courage, discovery, and the belief that power is meaningful only when guided by purpose.</p><p>Elijah Blaze Studios is developing one mythology across fiction, interactive play, and a living public archive. The work is transparent where it can inspire—and protected where mystery matters.</p></div>
        <div className="principles"><p>STUDIO PRINCIPLES</p>{[["01","WONDER BEFORE SCALE"],["02","COURAGE HAS A COST"],["03","SYSTEMS SERVE STORY"],["04","MYSTERY EARNS REVELATION"]].map(x=><div key={x[0]}><span>{x[0]}</span><h3>{x[1]}</h3><b>↗</b></div>)}</div>
        <div className="workflow"><div><small>ENGINE</small><b>UNITY</b><p>Playable systems and world-building.</p></div><div><small>WORKFLOW</small><b>CODEX-ASSISTED</b><p>Fast iteration with human direction.</p></div><div><small>METHOD</small><b>PUBLIC DEVELOPMENT</b><p>Share the path without spoiling the destination.</p></div></div>
      </section>

      <section className="signal section" id="signal">
        <div className="signal-orbit" aria-hidden="true"><span>◈</span></div><p className="kicker">05 / BLAZE SIGNAL</p><h2>THE SIGNAL<br/><i>IS OPEN.</i></h2><p>Development notes, recovered lore, prototype updates, and early pages from Beneath Eden—sent only when there is something worth discovering.</p>
        <form className="signup"><label className="sr-only" htmlFor="email">Email address</label><input id="email" type="email" placeholder="YOUR EMAIL ADDRESS" required/><button type="submit">JOIN THE SIGNAL <span>↗</span></button></form><small>NO NOISE. NO SPOILERS. TRANSMISSION ENCRYPTION: ACTIVE.</small>
      </section>

      <footer><a className="brand" href="#home"><span className="brand-mark">EB</span><span>ELIJAH BLAZE</span></a><p>AN ORIGINAL UNIVERSE BY ELIJAH BLAZE STUDIOS</p><div><a href="#game">GAME</a><a href="#books">BOOKS</a><a href="#codex">CODEX</a><a href="#studio">STUDIO</a></div><small>© 2026 ELIJAH BLAZE STUDIOS</small></footer>
    </main>
  );
}
