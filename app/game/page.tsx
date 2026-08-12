import Link from "next/link";

const systems = [
  ["Movement", "ONLINE", "Responsive third-person traversal for Hollow Court exploration."],
  ["Camera", "ONLINE", "Follow camera tuned for readability, combat, and discovery."],
  ["Shardling AI", "ONLINE", "Detection, chase, contact pressure, and combat engagement."],
  ["Combat", "ONLINE", "Close-range exchanges built around deliberate timing and clear impact."],
  ["Health", "ONLINE", "Shared damage framework for Elijah and Shardlings."],
  ["Death Feedback", "ACTIVE BUILD", "Visible defeat states, collapse effects, and player failure messaging."],
  ["Gate Objective", "NEXT", "Defeat the court’s threats and awaken the sealed architecture."],
];

export default function GamePage() {
  return (
    <main id="main-content">
      <header className="nav-shell">
        <Link className="brand" href="/"><span className="brand-mark">EB</span><span>ELIJAH BLAZE</span></Link>
        <nav aria-label="Game navigation"><Link href="/">Home</Link><Link href="/books">Books</Link><Link href="/comics">Comics</Link><a href="#roadmap">Roadmap</a></nav>
        <Link className="nav-signal" href="/#signal"><span/>SIGNAL LIVE</Link>
      </header>

      <section className="section game" style={{paddingTop:150}}>
        <div className="section-heading"><div><p className="kicker">PLAYABLE DEVELOPMENT</p><h2>THE HOLLOW<br/><i>COURT</i></h2></div><p className="lead">The first playable slice of Elijah Blaze: a place where architecture remembers, Shardlings hunt, and intention changes what the world allows.</p></div>
        <div className="game-grid"><div className="prototype-visual"><div className="reticle">◇</div><div className="hud"><span>UNITY 6 // PROTOTYPE</span><b>HOLLOW COURT</b></div></div><div className="milestone-card"><div className="card-top"><p>CURRENT BUILD</p><span>PRE-ALPHA</span></div><h3>Core Combat Loop</h3><p className="lead">Movement, camera, enemy pursuit, combat, and health all work. The current focus is making victory and defeat unmistakable, then turning the court into a complete objective-driven encounter.</p></div></div>
        <div className="system-strip">{systems.slice(0,3).map(([name,status,copy])=><article key={name}><span>{status}</span><h3>{name}</h3><p>{copy}</p></article>)}</div>
      </section>

      <section className="section codex" id="roadmap">
        <div className="section-heading"><div><p className="kicker">DEVELOPMENT ROADMAP</p><h2>FROM CUBES<br/><i>TO COURT.</i></h2></div><p className="lead">We are deliberately proving the game with simple geometry before replacing anything with final characters, animation, VFX, or architecture.</p></div>
        <div className="codex-grid">{systems.map(([name,status,copy],i)=><article key={name}><span className="entry-number">0{i+1}</span><p>{status}</p><h3>{name}</h3><p className="entry-copy">{copy}</p></article>)}</div>
      </section>

      <section className="section studio"><div className="studio-copy"><p className="kicker">DESIGN RULE</p><h2>DISCOVERY<br/><i>BEFORE LOOT.</i></h2><p className="manifesto">The player should ask “What is over there?” before asking “What do I get?”</p><p>Knowledge, resonance, access, and understanding are the real progression systems. AZEL does not simply make Elijah stronger. Their synchronization changes what he can perceive, understand, and awaken.</p></div><div className="principles"><p>CORE SYSTEMS</p>{[["01","KAVANAH"],["02","SYNCHRONIZATION"],["03","LIVING ARCHITECTURE"],["04","SHARDLING PRESSURE"]].map(x=><div key={x[0]}><span>{x[0]}</span><h3>{x[1]}</h3><b>↗</b></div>)}</div></section>
    </main>
  );
}
