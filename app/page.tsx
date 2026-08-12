import Image from "next/image";
import Link from "next/link";
import { CodexArchive } from "./CodexArchive";
import { SignalSignup } from "./SignalSignup";

const milestones = [
  ["Player movement", "complete"], ["Camera system", "complete"],
  ["Shardling chase", "complete"], ["Combat", "complete"],
  ["Health system", "complete"], ["Death feedback", "in progress"],
  ["Gate objective", "next"],
];

const saga = [
  ["Beneath Eden", "Curiosity"], ["The Guardian’s Dream", "Trust"],
  ["The Garden Below", "Discovery"], ["The Shattered Architecture", "Identity"],
  ["The Seven Echoes", "Sacrifice"], ["The Last Kavanah", "Choice"],
  ["The Flame Above Eden", "Restoration"],
];

export default function Home() {
  return (
    <main id="main-content">
      <a className="skip-link" href="#gateway">Skip to the universe</a>
      <header className="nav-shell">
        <a className="brand" href="#home" aria-label="Elijah Blaze home"><span className="brand-mark">EB</span><span>ELIJAH BLAZE</span></a>
        <nav aria-label="Primary navigation"><Link href="/game">Game</Link><Link href="/books">Books</Link><Link href="/comics">Comics</Link><a href="#codex">Codex</a><a href="#studio">Studio</a></nav>
        <a className="nav-signal" href="#signal"><span />SIGNAL LIVE</a>
      </header>

      <section className="hero" id="home">
        <div className="hero-atmosphere" aria-hidden="true"><div className="sun"/><div className="monolith"/><div className="signal-line"/><div className="guardian-seal"><span>◈</span><small>ARCHITECTURE // ONLINE</small></div></div>
        <div className="avatar-frame">
          <Image src="/assets/elijah-blaze-avatar.png" alt="Elijah Blaze in blue-and-gold guardian armor crowned by flame" fill sizes="(max-width: 560px) 105vw, (max-width: 900px) 68vw, 650px" priority />
          <div className="avatar-caption"><span>GUARDIAN FORM</span><b>ELIJAH + AZEL</b></div>
        </div>
        <div className="eyebrow"><span>PROJECT AZEL</span><span className="line"/><span>TRANSMISSION 001</span></div>
        <h1>ELIJAH<br/><em>BLAZE</em></h1>
        <p className="hero-tag">Some dreams are not your own.</p>
        <p className="hero-copy">Elijah Blue is sixteen, curious, and increasingly haunted by dreams of a place that should not exist. Beneath the Arizona desert waits AZEL—an ancient living architecture whose sleeping mind is slowly becoming entangled with his own.</p>
        <div className="hero-actions"><Link className="button primary" href="/books">Enter Beneath Eden <span>↗</span></Link><Link className="button text-button" href="/game">Enter the Game <span>↗</span></Link></div>
        <div className="hero-index"><span>16-YEAR-OLD DREAMER</span><span>ARIZONA / EARTH</span><span>SYNC STATUS: <b>EMERGING</b></span></div>
      </section>

      <aside className="transmission-band" aria-label="Current Elijah Blaze transmission"><span>TRANSMISSION // 001</span><p><strong>BENEATH EDEN</strong> The first door is opening.</p><a href="#signal">Receive the next signal <b>↗</b></a></aside>

      <section className="section codex" id="gateway">
        <div className="section-heading"><div><p className="kicker">THE ELIJAH BLAZE UNIVERSE</p><h2>ONE MYSTERY.<br/><i>THREE DOORS.</i></h2></div><p className="lead">Enter through the novels, the comics, or the playable Hollow Court. Each medium reveals a different layer of the same buried world.</p></div>
        <div className="system-strip">
          <article><span>01 / NOVELS</span><h3>Beneath Eden</h3><p>Follow Elijah Blue from the first impossible dreams to the awakening of AZEL.</p><Link className="button text-button" href="/books">Explore the saga ↗</Link></article>
          <article><span>02 / COMICS</span><h3>Stories beside the saga</h3><p>Recovered histories, alternate viewpoints, and visual stories from the hidden architecture.</p><Link className="button text-button" href="/comics">Open the comic slate ↗</Link></article>
          <article><span>03 / GAME</span><h3>The Hollow Court</h3><p>Step inside the prototype and experience the architecture as a place that reacts to intention.</p><Link className="button text-button" href="/game">Follow development ↗</Link></article>
        </div>
      </section>

      <section className="game section" id="game-preview">
        <div className="section-heading"><div><p className="kicker">01 / THE GAME</p><h2>THE HOLLOW<br/><i>COURT</i></h2></div><p className="lead">A third-person action prototype about intention, resonance, and surviving the first place that dreams back.</p></div>
        <div className="game-grid"><div className="prototype-visual"><div className="reticle">◇</div><div className="hud"><span>PROTOTYPE // UNITY</span><b>HOLLOW COURT</b></div></div><div className="milestone-card"><div className="card-top"><p>CURRENT MILESTONE</p><span>PRE-ALPHA</span></div><h3>Core Combat Loop</h3><div className="progress"><span/></div><p className="progress-label">5 OF 7 SYSTEMS ONLINE <b>71%</b></p><div className="milestones">{milestones.map(([name,status],i)=><div className="milestone" key={name}><span className={`status ${status.replace(' ','-')}`}>{status === 'complete' ? '✓' : status === 'in progress' ? '◒' : '○'}</span><span>{name}</span><small>{String(i+1).padStart(2,'0')} / 07</small></div>)}</div><Link className="button primary" href="/game">Full Game Dev Page ↗</Link></div></div>
      </section>

      <section className="books section" id="books-preview">
        <div className="book-art"><div className="book-cover"><p>A NOVEL</p><div className="glyph">◈</div><h3>BENEATH<br/><em>EDEN</em></h3><small>BOOK ONE OF THE ELIJAH BLAZE SAGA</small></div><div className="book-shadow"/></div>
        <div className="book-copy"><p className="kicker">02 / THE BOOKS</p><h2>Before Elijah Blaze,<br/>there was <i>Elijah Blue.</i></h2><blockquote>“Some dreams are not your own.”</blockquote><p>Book One begins a seven-part saga of discovery, moral courage, and an impossible relationship between a boy and the intelligence sleeping below his world.</p><div className="book-meta"><span><small>FORMAT</small>NOVEL</span><span><small>SERIES</small>01 / 07</span><span><small>STATUS</small>IN DEVELOPMENT</span></div><Link className="button primary" href="/books">Open Beneath Eden <span>↗</span></Link></div>
        <div className="saga-roadmap"><p>THE SEVEN-BOOK SAGA</p>{saga.map(([title,theme],i)=><article className={i===0?'active':''} key={title}><span>0{i+1}</span><div><b>{title}</b></div><small>{theme}</small></article>)}</div>
      </section>

      <section className="codex section" id="codex">
        <div className="section-heading"><div><p className="kicker">03 / BLAZE CODEX</p><h2>THE ARCHIVE<br/><i>REMEMBERS.</i></h2></div><p className="lead">Recovered people, places, systems, and symbols from inside the Elijah Blaze universe. Some knowledge remains deliberately sealed.</p></div>
        <CodexArchive />
        <div className="archive-lock"><span>◈</span><div><p>RESTRICTED STRATUM</p><h3>ARCHIVE SEALED — SYNCHRONIZATION REQUIRED</h3></div><small>ACCESS LEVEL 07</small></div>
      </section>

      <section className="studio section" id="studio">
        <div className="studio-copy"><p className="kicker">04 / THE STUDIO</p><h2>WORLDS WITH<br/><i>PURPOSE.</i></h2><p className="manifesto">Wonder before spectacle. Character before power. Discovery before reward.</p><p>Elijah Blaze Studios is developing one mythology across fiction, comics, interactive play, and a living public archive. Codex accelerates implementation; human direction protects the world’s meaning and consistency.</p></div>
        <div className="principles"><p>STUDIO PRINCIPLES</p>{[["01","WONDER BEFORE SPECTACLE"],["02","CHARACTER BEFORE POWER"],["03","DISCOVERY BEFORE REWARD"],["04","EVERY MYSTERY HAS AN ANSWER"]].map(x=><div key={x[0]}><span>{x[0]}</span><h3>{x[1]}</h3><b>↗</b></div>)}</div>
        <div className="workflow"><div><small>ENGINE</small><b>UNITY 6</b><p>Playable systems and world-building.</p></div><div><small>WORKFLOW</small><b>CODEX-ASSISTED</b><p>Fast implementation with a locked creative bible.</p></div><div><small>METHOD</small><b>PUBLIC DEVELOPMENT</b><p>Share the path without spoiling the destination.</p></div></div>
      </section>

      <section className="signal section" id="signal"><div className="signal-orbit" aria-hidden="true"><span>◈</span></div><p className="kicker">05 / BLAZE SIGNAL</p><h2>THE SIGNAL<br/><i>IS OPEN.</i></h2><p>Development notes, recovered lore, prototype updates, comic reveals, and early pages from Beneath Eden—sent only when there is something worth discovering.</p><SignalSignup /></section>

      <footer><a className="brand" href="#home"><span className="brand-mark">EB</span><span>ELIJAH BLAZE</span></a><p>REMEMBER WHAT WAS FORGOTTEN.</p><div><Link href="/game">GAME</Link><Link href="/books">BOOKS</Link><Link href="/comics">COMICS</Link><a href="#codex">CODEX</a></div><small>© 2026 ELIJAH BLAZE STUDIOS</small></footer>
    </main>
  );
}
