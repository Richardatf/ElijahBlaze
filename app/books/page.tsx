import Link from "next/link";

const saga = [
  ["01","Beneath Eden","Curiosity","Elijah Blue begins dreaming of places he has never seen and follows an anomaly toward West Ridge."],
  ["02","The Guardian’s Dream","Trust","The connection becomes two-way. Elijah and AZEL begin to dream each other."],
  ["03","The Garden Below","Discovery","The buried world opens, and AZEL is revealed as far more than a machine."],
  ["04","The Shattered Architecture","Identity","Damage within the ancient system forces Elijah to ask where he ends and AZEL begins."],
  ["05","The Seven Echoes","Sacrifice","Seven surviving echoes reveal incompatible pieces of the forgotten age."],
  ["06","The Last Kavanah","Choice","Power reaches its limit; intention decides what the architecture will become."],
  ["07","The Flame Above Eden","Restoration","The hidden truth reaches the surface, and Elijah must restore without ruling."],
];

export default function BooksPage(){
 return <main id="main-content">
  <header className="nav-shell"><Link className="brand" href="/"><span className="brand-mark">EB</span><span>ELIJAH BLAZE</span></Link><nav><Link href="/">Home</Link><Link href="/game">Game</Link><Link href="/comics">Comics</Link><a href="#saga">Saga</a></nav><Link className="nav-signal" href="/#signal"><span/>SIGNAL LIVE</Link></header>
  <section className="books section" style={{paddingTop:150}}><div className="book-art"><div className="book-cover"><p>BOOK ONE</p><div className="glyph">◈</div><h3>BENEATH<br/><em>EDEN</em></h3><small>THE ELIJAH BLAZE SAGA</small></div><div className="book-shadow"/></div><div className="book-copy"><p className="kicker">THE NOVEL</p><h2>Some dreams<br/>are <i>not your own.</i></h2><blockquote>Before Elijah Blaze, there was Elijah Blue.</blockquote><p>Sixteen-year-old Elijah Blue has always been curious about the empty places on maps. Then his dreams begin showing him places that should not exist. The deeper he follows the mystery, the more impossible the question becomes: is something beneath the desert reaching into his mind—or has part of him always belonged there?</p><div className="book-meta"><span><small>BOOK</small>01 / 07</span><span><small>GENRE</small>YA MYSTERY / SF</span><span><small>STATUS</small>IN DEVELOPMENT</span></div></div>
  <div className="saga-roadmap" id="saga"><p>THE SEVEN BOOKS</p>{saga.map(([num,title,theme,copy],i)=><article className={i===0?"active":""} key={title}><span>{num}</span><div><b>{title}</b><p>{copy}</p></div><small>{theme}</small></article>)}</div></section>
  <section className="section codex"><div className="section-heading"><div><p className="kicker">BOOK ONE PROMISE</p><h2>MYSTERY<br/><i>BEFORE MECH.</i></h2></div><p className="lead">Readers meet Elijah Blue before they meet Elijah Blaze. AZEL is felt before he is seen. The bond begins in dreams long before either understands what is happening.</p></div><div className="system-strip"><article><span>ELIJAH</span><h3>Curious before chosen.</h3><p>His persistence earns discovery; destiny does not hand it to him.</p></article><article><span>AZEL</span><h3>Architecture before armor.</h3><p>The guardian form is only one expression of a much larger living system.</p></article><article><span>THE BOND</span><h3>Symbiosis before power.</h3><p>Each mind changes the other. Neither remains merely what it was.</p></article></div></section>
 </main>
}
